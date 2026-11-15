using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Behaviors;
using NutritionService.Data;
using NutritionService.Domain.Repositories;
using System.Reflection;

namespace NutritionService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNutritionServices(this IServiceCollection service, IConfiguration config)
        {
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddDbContext<NutritionDbContext>(opt =>
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            var assembly = Assembly.GetExecutingAssembly();
            service.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            service.AddValidatorsFromAssembly(assembly);
            return service;
        }
    }
}
