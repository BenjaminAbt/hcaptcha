// Copyright Â© BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Net;
using BenjaminAbt.HCaptcha.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace BenjaminAbt.HCaptcha.UnitTests;

public class VerificationTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task PostsEncodedCredentialsAndAppliesIpPolicy(bool verifyIp)
    {
        IOptions<HCaptchaOptions> options = Options.Create(
            new HCaptchaOptions { Secret = "s&+", SiteKey = "site", VerifyRemoteIp = verifyIp });
        using HttpClient client = new(new Handler(async request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://api.hcaptcha.com/siteverify", request.RequestUri!.AbsoluteUri);
            Assert.Equal("application/x-www-form-urlencoded", request.Content!.Headers.ContentType!.MediaType);
            Dictionary<string, StringValues> form = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(
                await request.Content.ReadAsStringAsync());
            Assert.Equal("s&+", form["secret"].ToString());
            Assert.Equal("t=+", form["response"].ToString());
            Assert.Equal("site", form["sitekey"].ToString());
            Assert.Equal(verifyIp ? "127.0.0.1" : "", form.GetValueOrDefault("remoteip").ToString());
            return new(HttpStatusCode.OK) { Content = new StringContent("""{"success":true}""") };
        }))
        { BaseAddress = new Uri(options.Value.ApiBaseUrl) };

        HCaptchaProvider provider = new(client, options);
        Assert.True((await provider.VerifyAsync("t=+", "127.0.0.1", TestContext.Current.CancellationToken)).Success);
    }

    [Theory]
    [InlineData(HttpStatusCode.ServiceUnavailable, "{}")]
    [InlineData(HttpStatusCode.OK, "invalid")]
    [InlineData(HttpStatusCode.OK, "null")]
    public async Task RejectsUnusableApiResponses(HttpStatusCode status, string body)
    {
        using HttpClient client = new(new Handler(_ => Task.FromResult(new HttpResponseMessage(status)
        { Content = new StringContent(body) })))
        { BaseAddress = new Uri("https://api.hcaptcha.com/") };
        HCaptchaProvider provider = new(client, Options.Create(new HCaptchaOptions()));
        HCaptchaApiException error = await Assert.ThrowsAsync<HCaptchaApiException>(() =>
            provider.VerifyAsync("token", cancellationToken: TestContext.Current.CancellationToken));
        Assert.Equal(status, error.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public async Task BinderRejectsMissingDuplicateAndDeniedTokens(int count)
    {
        DefaultModelBindingContext context = CreateFormContext(Enumerable.Repeat("token", count).ToArray());
        RecordingProvider provider = new();
        await new HCaptchaModelBinder(provider, Options.Create(new HCaptchaOptions())).BindModelAsync(context);
        Assert.False(context.ModelState.IsValid);
        Assert.Equal(count == 1, provider.Called);
        Assert.Equal(count == 1, context.Result.IsModelSet);
    }

    [Fact]
    public async Task BinderRequiresAFormPost()
    {
        DefaultModelBindingContext context = CreateFormContext("token");
        RecordingProvider provider = new();
        HCaptchaModelBinder binder = new(provider, Options.Create(new HCaptchaOptions()));
        context.HttpContext.Request.Method = "GET";
        await Assert.ThrowsAsync<InvalidOperationException>(() => binder.BindModelAsync(context));
        context.ActionContext.HttpContext = new DefaultHttpContext();
        context.HttpContext.Request.Method = "POST";
        context.HttpContext.Request.ContentType = "application/json";
        await binder.BindModelAsync(context);
        Assert.False(context.ModelState.IsValid);
        Assert.False(provider.Called);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task BinderFailsClosedWhenVerificationIsUnavailable(bool timedOut)
    {
        DefaultModelBindingContext context = CreateFormContext("token");
        HCaptchaModelBinder binder = new(new UnavailableProvider(timedOut),
            Options.Create(new HCaptchaOptions()));
        await binder.BindModelAsync(context);
        Assert.False(context.Result.IsModelSet);
        Assert.Contains("unavailable", Assert.Single(context.ModelState["captcha"]!.Errors).ErrorMessage);
    }

    [Fact]
    public async Task BinderPassesClientIpAndCancellationAndBindsVerifiedResponse()
    {
        DefaultModelBindingContext context = CreateFormContext("token");
        context.HttpContext.Connection.RemoteIpAddress = IPAddress.Loopback;
        context.HttpContext.RequestAborted = TestContext.Current.CancellationToken;
        RecordingProvider provider = new(success: true);
        HCaptchaModelBinder binder = new(provider, Options.Create(new HCaptchaOptions()));

        await binder.BindModelAsync(context);

        Assert.Equal("127.0.0.1", provider.RemoteIp);
        Assert.Equal(context.HttpContext.RequestAborted, provider.CancellationToken);
        Assert.True(context.ModelState.IsValid);
        Assert.True(Assert.IsType<HCaptchaVerifyResponse>(context.Result.Model).Success);
    }

    private static DefaultModelBindingContext CreateFormContext(params string[] tokens)
    {
        DefaultHttpContext http = new();
        http.Request.Method = "POST";
        http.Request.ContentType = "application/x-www-form-urlencoded";
        http.Request.Form = new FormCollection(new Dictionary<string, StringValues>
        {
            ["h-captcha-response"] = tokens
        });
        return new DefaultModelBindingContext
        {
            ActionContext = new Microsoft.AspNetCore.Mvc.ActionContext { HttpContext = http },
            ModelName = "captcha",
            ModelState = new()
        };
    }

    private sealed class UnavailableProvider(bool timedOut) : IHCaptchaProvider
    {
        public Task<HCaptchaVerifyResponse> VerifyAsync(string token, string? remoteIp,
            CancellationToken cancellationToken)
        {
            throw timedOut ? new TaskCanceledException() : new HttpRequestException();
        }
    }

    private sealed class Handler(Func<HttpRequestMessage, Task<HttpResponseMessage>> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return respond(request);
        }
    }

    private sealed class RecordingProvider(bool success = false) : IHCaptchaProvider
    {
        public bool Called { get; private set; }
        public string? RemoteIp { get; private set; }
        public CancellationToken CancellationToken { get; private set; }

        public Task<HCaptchaVerifyResponse> VerifyAsync(string token, string? remoteIp,
            CancellationToken cancellationToken)
        {
            Called = true;
            RemoteIp = remoteIp;
            CancellationToken = cancellationToken;
            return Task.FromResult(new HCaptchaVerifyResponse { Success = success });
        }
    }
}
