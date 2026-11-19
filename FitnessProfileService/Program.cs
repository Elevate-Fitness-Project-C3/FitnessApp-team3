using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Infrastructure;
using FitnessProfileService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Services
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddAuthorization();

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

app.MapControllers();

// 4. Apply EF Core Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await dbContext.Database.MigrateAsync();
}

// 5. Run Application
app.Run();