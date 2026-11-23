using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NutritionService.Comman.Behaviors;
using NutritionService.Data;
using NutritionService.Domain.Repositories;
using NutritionService.Features.CalorieTarget.DTOs;
using NutritionService.Features.CalorieTarget.Handlers;
using NutritionService.Features.CalorieTarget.Queries;
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

            service.AddHttpClient<IRequestHandler<GetUserTdeeQuery, FitnessProfileTdeeDto>, GetUserTdeeQueryHandler>(client =>
            {
                var baseUrl = config["Services:FitnessProfileBaseUrl"]
                  ?? "http://localhost:5007";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(5);
            })
            .AddHttpMessageHandler<BearerTokenForwardingHandler>();

            return service;
        }
    }
}