
using exam_system.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using TrainingService.Infrastructure.Context;
using TrainingService.Infrastructure.DataAccess;

namespace exam_system.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
                                                 ?? throw new InvalidOperationException(
                                                 "DefaultConnection is missing.");


        services.AddDbContext<TrainingDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"));
        });


        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
