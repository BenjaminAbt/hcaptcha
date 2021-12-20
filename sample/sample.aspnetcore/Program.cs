// Copyright © Benjamin Abt 2020-2021, all rights reserved

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
