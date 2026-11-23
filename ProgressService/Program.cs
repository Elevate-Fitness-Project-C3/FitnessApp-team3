using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProgressService.Domains.Repositories;
using ProgressService.Feature.CompletedWorkout.Consumers;
using ProgressService.Feature.CompletedWorkout.Queries;
using ProgressService.Feature.CompletedWorkout.Validator;
using ProgressService.Feature.Shared;
using ProgressService.Feature.Shared.EventContracts;
using ProgressService.Infrastructure.Persistence;
using ProgressService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddDbContext<ProgressDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<GetWorkoutHistoryValidator>();

builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssemblyContaining<WorkoutCompletedConsumer>());

builder.Services.AddMassTransit(config => {
    config.AddConsumer<WorkoutCompletedConsumer>();
    config.AddConfigureEndpointsCallback((context, _, endpoint) =>
    {
        endpoint.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(5)));
        endpoint.UseEntityFrameworkOutbox<ProgressDbContext>(context);
    });

    config.AddEntityFrameworkOutbox<ProgressDbContext>(o =>
    {
        o.UseSqlServer();
        // No Bus
        o.DuplicateDetectionWindow = TimeSpan.FromMinutes(30);
    });

    // Configure RabbitMQ here
    config.UsingRabbitMq((context, cfg) => {
        cfg.Host(builder.Configuration["RabbitMq:Host"], builder.Configuration["RabbitMq:VirtualHost"], h =>
        {
            h.Username(builder.Configuration["RabbitMq:Username"]!);
            h.Password(builder.Configuration["RabbitMq:Password"]!);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ProgressDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/progress/history", async Task<IResult> (
    HttpContext httpContext,
    GetWorkoutHistoryValidator validator,
    IMediator mediator,
    CancellationToken cancellationToken,
    int page = 1,
    int pageSize = 20) =>
{
    //if (!CurrentUser.TryGetId(httpContext.User, out var userId))
    //    return Results.Unauthorized();
    // How To generate the user Id
    Guid userId = new Guid();

    var query = new GetWorkoutHistoryQuery(userId, page, pageSize);
    var validation = await validator.ValidateAsync(query, cancellationToken);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    return Results.Ok(await mediator.Send(query, cancellationToken));
});

app.MapGet("/api/progress/stats", async Task<IResult> (
    HttpContext httpContext,
    IMediator mediator,
    CancellationToken cancellationToken) =>
{
    if (!CurrentUser.TryGetId(httpContext.User, out var userId))
        return Results.Unauthorized();

    return Results.Ok(await mediator.Send(
        new GetProgressStatsQuery(userId), cancellationToken));
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapPost("/test/workout-completed", async (
    WorkoutCompleted message,
    IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(message);
    return Results.Accepted();
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
