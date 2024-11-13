// Copyright © Benjamin Abt 2020-2024, all rights reserved

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>
/// Provides extension methods for registering hCaptcha services in the dependency injection container.
/// </summary>
public static class HCaptchaExtensions
{
    /// <summary>
    /// Adds hCaptcha services to the <see cref="IServiceCollection"/> using the provided configuration section.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the hCaptcha services will be added.</param>
    /// <param name="section">The <see cref="IConfigurationSection"/> containing the hCaptcha configuration settings.</param>
    /// <returns>The <see cref="IServiceCollection"/> with hCaptcha services added.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="services"/> or <paramref name="section"/> is <c>null</c>.
    /// </exception>
    public static IServiceCollection AddHCaptcha(this IServiceCollection services, IConfigurationSection section)
    {
        // Bind configuration to HCaptchaOptions
        HCaptchaOptions captchaOptions = new();
        section.Bind(captchaOptions);

        // Configure options to be injected wherever needed
        services.Configure<HCaptchaOptions>(section);

        // Register the Refit client for IHCaptchaApi with the base URL from configuration
        services.AddRefitClient<IHCaptchaApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(captchaOptions.ApiBaseUrl);
            });

        // Register the hCaptcha provider
        services.AddScoped<IHCaptchaProvider, HCaptchaProvider>();

        return services;
    }
}
