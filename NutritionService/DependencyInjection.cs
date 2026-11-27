using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Behaviors;
using NutritionService.Data;
using NutritionService.Domain.Repositories;
using NutritionService.Features.CalorieTarget.Handlers;
using NutritionService.Options;
using NutritionService.Services.FitnessProfile;
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

            //================================================================
            service.Configure<CalorieTargetOptions>(config.GetSection(CalorieTargetOptions.SectionName));
            service.AddHttpContextAccessor();
            service.AddTransient<BearerTokenForwardingHandler>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();

            service.AddHttpClient(
             GetUserTdeeQueryHandler.ClientName,
             client =>
             {
                 var baseUrl = config["Services:FitnessProfileBaseUrl"];
                 if (string.IsNullOrWhiteSpace(
                     baseUrl))
                    {
                     baseUrl = "http://localhost:5007/";
                 }

                 client.BaseAddress = new Uri(baseUrl);
                 client.Timeout =
                     TimeSpan.FromSeconds(5);
             }).AddHttpMessageHandler<BearerTokenForwardingHandler>();

            return service;
        }
    }
}