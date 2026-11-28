using exam_system.Persistence;
using FitnessApp.Authentication;
using Microsoft.EntityFrameworkCore;
using TrainingService.Features.Exercises;
using TrainingService.Features.TrainingPrograms;
using TrainingService.Features.Workouts;
using TrainingService.Infrastructure.Context;
using TrainingService.Infrastructure.Exceptions;


var builder = WebApplication.CreateBuilder(args);
if (!EF.IsDesignTime)
    builder.Services.AddAccessTokenAuthentication(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddPersistenceServices(
    builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();


var app = builder.Build();

app.UseExceptionHandler();


if (!EF.IsDesignTime)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TrainingDbContext>();
    await dbContext.Database.MigrateAsync();
}

//For Apis
app.MapExerciseEndpoints();
app.MapWorkoutEndpoints();
app.MapTrainingProgramEndpoints();
app.MapProgramWorkoutEndpoints();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Training Service API");
    });
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

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
.WithName("GetWeatherForecast")
.RequireAuthorization();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
