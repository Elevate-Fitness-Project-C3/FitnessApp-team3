using System.Reflection;
using FitnessApp.Authentication;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Behaviors;
using NutritionService.Data;
using NutritionService.Domain.Repositories;
using NutritionService.Features.CalorieTarget.Consumers;
using NutritionService.Options;

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

            service.Configure<CalorieTargetOptions>(config.GetSection(CalorieTargetOptions.SectionName));
            service.Configure<MealRecommendationOptions>(config.GetSection(MealRecommendationOptions.SectionName));
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddAccessTokenAuthentication(config);
            service.AddMessaging(config);

            return service;
        }

        private static IServiceCollection AddMessaging(this IServiceCollection service, IConfiguration config)
        {
            service.AddMassTransit(x =>
            {
                x.AddConsumer<UserTdeeCalculatedConsumer>()
                    .Endpoint(e => e.Name = "nutrition-user-tdee-calculated");

                x.AddEntityFrameworkOutbox<NutritionDbContext>(o =>
                {
                    o.UseSqlServer();
                });

                x.AddConfigureEndpointsCallback((context, _, cfg) =>
                {
                    cfg.UseMessageRetry(r => r.Intervals(
                        TimeSpan.FromSeconds(1),
                        TimeSpan.FromSeconds(5),
                        TimeSpan.FromSeconds(15)));

                    cfg.UseEntityFrameworkOutbox<NutritionDbContext>(context);
                });

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(
                        config["RabbitMq:Host"] ?? "rabbitmq",
                        config["RabbitMq:VirtualHost"] ?? "/",
                        h =>
                        {
                            h.Username(config["RabbitMq:Username"] ?? "guest");
                            h.Password(config["RabbitMq:Password"] ?? "guest");
                        });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return service;
        }
    }
}