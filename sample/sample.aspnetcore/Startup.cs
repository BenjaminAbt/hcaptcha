// Copyright © Benjamin Abt 2020-2024, all rights reserved

using BenjaminAbt.HCaptcha.AspNetCore;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore;

/// <summary>
/// Configures services and the HTTP request pipeline for the application.
/// </summary>
public class Startup
{
    /// <summary>
    /// Gets the configuration settings for the application.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration settings for the application.</param>
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// Configures services for dependency injection in the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which services are added.</param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Add hCaptcha services using the configuration from the "HCaptcha" section
        services.AddHCaptcha(Configuration.GetSection("HCaptcha"));

        // Add MVC controllers with views and configure the hCaptcha model binder
        services.AddControllersWithViews(mvcOptions =>
            // Add custom model binder for hCaptcha
            mvcOptions.AddHCaptchaModelBinder());
    }

    /// <summary>
    /// Configures the HTTP request pipeline for the application.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> used to configure the middleware pipeline.</param>
    /// <param name="env">The <see cref="IWebHostEnvironment"/> containing information about the hosting environment.</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Use the developer exception page in development
            app.UseDeveloperExceptionPage();
        }

        // Enable routing in the middleware pipeline
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            // Map MVC controllers to their respective endpoints
            endpoints.MapControllers();
        });
    }
}
