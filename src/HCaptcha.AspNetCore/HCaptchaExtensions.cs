// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BenjaminAbt.HCaptcha.AspNetCore;

public static class HCaptchaExtensions
{
    /// <summary>
    /// Adds the HCaptcha configuration <see cref="section"/> as <see cref="HCaptchaOptions"/>.
    /// Adds <see cref="IHCaptchaApi"/> as Refit Client with given base address in <see cref="HCaptchaOptions"/>.
    /// </summary>
    public static IServiceCollection AddHCaptcha(this IServiceCollection services, IConfigurationSection section)
    {
        HCaptchaOptions captchaOptions = new();
        section.Bind(captchaOptions);

        services.Configure<HCaptchaOptions>(section);

        services.AddRefitClient<IHCaptchaApi>()
            .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(captchaOptions.ApiBaseUrl);
                }
            );

        services.AddScoped<IHCaptchaProvider, HCaptchaProvider>();

        return services;
    }
}
