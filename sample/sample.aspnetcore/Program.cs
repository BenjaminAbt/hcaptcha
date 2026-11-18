// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using BenjaminAbt.HCaptcha.AspNetCore;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore;

internal static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHCaptcha(builder.Configuration.GetRequiredSection("HCaptcha"));
        builder.Services.AddControllersWithViews(options => options.AddHCaptchaModelBinder());

        WebApplication app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}
