// Copyright © Benjamin Abt 2020-2024, all rights reserved

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore;

/// <summary>
/// The entry point for the application, responsible for setting up and running the web host.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    public static void Main(string[] args)
    {
        // Build and run the host
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates and configures the <see cref="IHostBuilder"/> for the application.
    /// </summary>
    /// <param name="args">The command-line arguments passed to the application.</param>
    /// <returns>An <see cref="IHostBuilder"/> that configures the web host.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Use the Startup class for configuring services and middleware
                webBuilder.UseStartup<Startup>();
            });
}
