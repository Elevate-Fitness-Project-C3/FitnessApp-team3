using IdentityService.Domain.Repositories;
using FluentValidation;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Identity.Register.Orchestrators;
using IdentityService.Features.Identity.Register.Validators;
using IdentityService.Features.Identity.VerifyEmailOtp.Validators;
using IdentityService.Features.Identity.VerifyEmailOtp.ViewModels;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Repositories;
using IdentityService.Infrastructure.Services;
using IdentityService.Infrastructure.Settings;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityRegistration(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SmtpSettings>().Bind(configuration.GetSection(SmtpSettings.SectionName))
            .Validate(settings => settings.IsValid(), "SMTP settings are missing or invalid.")
            .ValidateOnStart();
        services.AddOptions<OtpSettings>().Bind(configuration.GetSection(OtpSettings.SectionName))
            .Validate(settings => settings.Length is >= 6 and <= 8
                && settings.ExpirationMinutes is >= 1 and <= 30
                && settings.MaxFailedAttempts is >= 1 and <= 10
                && settings.HasValidProtectionKey(), "OTP settings are missing or invalid.")
            .ValidateOnStart();

        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddTransient<ISmtpClient, SmtpClient>();
        services.AddScoped<IEmailSender, MailKitEmailSender>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<VerifyEmailOtpVM>, VerifyEmailOtpVMValidator>();
        services.AddScoped<IValidator<ResendEmailVerifycationOtpVM>, ResendEmailVerifycationOtpVMValidator>();
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssemblyContaining<RegisterUserOrchestrator>());
        return services;
    }

    public static IServiceCollection AddIdentityPersistence(
        this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureOptions)
    {
        services.AddDbContext<AppDbContext>(configureOptions);
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
