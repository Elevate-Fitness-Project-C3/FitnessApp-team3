using Microsoft.EntityFrameworkCore;
using NutritionService.Domain.Entities;
using NutritionService.Domain.Enums;

namespace NutritionService.Data
{
    public static class NutritionDbSeeder 
    {
        public static async Task InitializeDatabaseAsync (this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NutritionDbContext>();

            await db.Database.MigrateAsync();

            if (await db.Meals.AnyAsync())
                return;
            db.Meals.AddRange(
                Meal.Create("Oatmeal with Banana", MealType.Breakfast, 380, 14).Value,
                Meal.Create("Grilled Chicken with Rice", MealType.Lunch, 620, 48).Value,
                Meal.Create("Greek Yogurt with Berries", MealType.Snack, 220, 18).Value
            );
            await db.SaveChangesAsync();
        }
    }
    
}
