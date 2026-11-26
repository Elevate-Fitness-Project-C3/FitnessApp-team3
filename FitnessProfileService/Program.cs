
using FitnessProfileService.Common.Behaviors;
using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.review_body_measurements.EndPoints;
using FitnessProfileService.Features.Fitness__OnBoarding___Assessment.View_and_update_fitness_profile.Endpoints;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Endpoints;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Handlers;
using FitnessProfileService.Features.OnBoarding.CompeleteOnBoarding.Orchestrators;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Commands;
using FitnessProfileService.Features.OnBoarding.Set_training_preferences.Endpoints;
using FitnessProfileService.Infrastructure;
using FitnessProfileService.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Services
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Register MediatR and Validation Pipeline
builder.Services.AddMediatR(
    typeof(CompleteOnboardingOrchestratorHandler).Assembly);

builder.Services.AddValidatorsFromAssembly(
    typeof(UpdateTrainingPreferencesCommand).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));



// Register Persistence
builder.Services.AddIdentityPersistence(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// 2. Build Application
var app = builder.Build();

// 3. Configure Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Map Minimal API Endpoints
app.MapCompleteOnboardingEndpoint();
app.MapUpdateTrainingPreferencesEndpoint();
app.MapViewFintnessProfile();
app.MapUpdateBodyMeasurement();
app.MapBodyMeasurements();

// 4. Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();
}

// 5. Run Application
app.Run();
