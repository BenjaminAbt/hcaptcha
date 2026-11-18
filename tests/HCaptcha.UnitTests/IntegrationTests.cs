// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using BenjaminAbt.HCaptcha.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace BenjaminAbt.HCaptcha.UnitTests;

public class IntegrationTests
{
    [Fact]
    public async Task MvcSelectsTheCaptchaBinderOnlyForVerificationResponses()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Configuration["HCaptcha:SiteKey"] = "site";
        builder.Configuration["HCaptcha:Secret"] = "secret";
        builder.Services.AddHCaptcha(builder.Configuration.GetSection("HCaptcha"));
        builder.Services.AddControllers(options => options.AddHCaptchaModelBinder(o => o.BinderPosition = 1));

        await using WebApplication app = builder.Build();
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        Assert.IsType<HCaptchaProvider>(services.GetRequiredService<IHCaptchaProvider>());
        IModelMetadataProvider metadata = services.GetRequiredService<IModelMetadataProvider>();
        IModelBinderFactory factory = services.GetRequiredService<IModelBinderFactory>();
        IModelBinder Bind(Type type)
        {
            return factory.CreateBinder(new ModelBinderFactoryContext
            {
                Metadata = metadata.GetMetadataForType(type)
            });
        }

        Assert.IsType<BinderTypeModelBinder>(Bind(typeof(HCaptchaVerifyResponse)));
        Assert.IsNotType<BinderTypeModelBinder>(Bind(typeof(string)));
    }

    [Theory]
    [InlineData("", "secret", "https://api.hcaptcha.com", "SiteKey")]
    [InlineData("site", " ", "https://api.hcaptcha.com", "Secret")]
    [InlineData("site", "secret", "http://api.hcaptcha.com", "HTTPS")]
    [InlineData("site", "secret", "relative", "HTTPS")]
    public async Task StartupValidationRejectsInvalidConfiguration(
        string site, string secret, string url, string reason)
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Logging.ClearProviders();
        ConfigurationManager config = builder.Configuration;
        config["HCaptcha:SiteKey"] = site;
        config["HCaptcha:Secret"] = secret;
        config["HCaptcha:ApiBaseUrl"] = url;
        builder.Services.AddHCaptcha(config.GetSection("HCaptcha"));
        using IHost host = builder.Build();

        OptionsValidationException error = await Assert.ThrowsAsync<OptionsValidationException>(
            () => host.StartAsync(TestContext.Current.CancellationToken));
        Assert.Contains(reason, error.Message);
    }

    [Fact]
    public void VerificationErrorsPreserveOrderAndIdentifyUnrecognizedCodes()
    {
        HCaptchaVerifyResponse response = new();
        Assert.Empty(response.GetErrorCodes());

        response.ErrorCodes = ["expired-input-response", "future-code", "sitekey-secret-mismatch"];
        Assert.Equal([HCaptchaVerifyErrorCode.ExpiredInputResponse, HCaptchaVerifyErrorCode.Unknown,
            HCaptchaVerifyErrorCode.SiteKeySecretMismatch], response.GetErrorCodes());
    }
}
