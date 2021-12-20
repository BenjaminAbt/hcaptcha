// Copyright © Benjamin Abt 2020-2021, all rights reserved

using BenjaminAbt.HCaptcha.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // HCaptcha
        services.AddHCaptcha(Configuration.GetSection("HCaptcha"));

        // Mvc
        services.AddControllersWithViews(mvcOptions =>
            // add model binder
            mvcOptions.AddHCaptchaModelBinder());
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            // Mvc
            endpoints.MapControllers();
        });
    }
}
