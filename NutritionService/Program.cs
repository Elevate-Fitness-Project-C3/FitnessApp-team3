using FitnessApp.Authentication;
using Microsoft.EntityFrameworkCore;
using NutritionService;
using NutritionService.Data;
using NutritionService.EndPoints;

var builder = WebApplication.CreateBuilder(args);
if (!EF.IsDesignTime)
    builder.Services.AddAccessTokenAuthentication(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddNutritionServices(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
await app.InitializeDatabaseAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapNutritionPreferenceEndpoints();
app.MapCalorieTargetEndpoints();
app.MapMealEndpoints();
app.MapRecommendationEndpoints();
app.Run();