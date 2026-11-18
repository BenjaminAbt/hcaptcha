// Copyright Â© BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>Registers hCaptcha verification services.</summary>
public static class HCaptchaExtensions
{
    /// <summary>Registers the verification client and validates configuration at startup.</summary>
    public static IServiceCollection AddHCaptcha(this IServiceCollection services, IConfigurationSection section)
    {
        services.AddOptions<HCaptchaOptions>().Bind(section)
            .Validate(o => !string.IsNullOrWhiteSpace(o.SiteKey), "HCaptcha SiteKey is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.Secret), "HCaptcha Secret is required.")
            .Validate(o => Uri.TryCreate(o.ApiBaseUrl, UriKind.Absolute, out Uri? uri)
                && uri.Scheme == Uri.UriSchemeHttps,
                "HCaptcha ApiBaseUrl must be an absolute HTTPS URL.")
            .ValidateOnStart();

        services.AddHttpClient<IHCaptchaProvider, HCaptchaProvider>((sp, client) =>
        {
            client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<HCaptchaOptions>>().Value.ApiBaseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
        }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            AllowAutoRedirect = false,
            UseCookies = false
        });
        return services;
    }
}
