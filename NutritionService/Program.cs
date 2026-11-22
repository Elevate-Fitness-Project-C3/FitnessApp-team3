using Microsoft.EntityFrameworkCore;
using NutritionService;
using NutritionService.Data;
using NutritionService.EndPoints;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAccessTokenAuthentication(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddNutritionServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<NutritionDbContext>();
    await db.Database.MigrateAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapNutritionPreferenceEndpoints();
app.MapCalorieTargetEndpoints();
app.MapMealEndpoints();
app.Run();


