// Copyright Â© BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>Binds a server-verified challenge to an MVC action parameter.</summary>
public class HCaptchaModelBinder(IHCaptchaProvider captchaProvider, IOptions<HCaptchaOptions> captchaOptionsAccessor)
    : IModelBinder
{
    /// <inheritdoc />
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);
        HttpContext context = bindingContext.HttpContext;
        if (!HttpMethods.IsPost(context.Request.Method))
        {
            throw new InvalidOperationException("hCaptcha model binding requires a POST request.");
        }

        void Fail(string message)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, message);
            bindingContext.Result = ModelBindingResult.Failed();
        }

        if (!context.Request.HasFormContentType)
        {
            Fail("Expected a form containing an hCaptcha response.");
            return;
        }

        IFormCollection form = await context.Request.ReadFormAsync(context.RequestAborted);
        StringValues tokens = form[captchaOptionsAccessor.Value.HttpPostResponseKeyName];
        if (tokens.Count != 1 || string.IsNullOrWhiteSpace(tokens[0]))
        {
            Fail("A single hCaptcha response is required.");
            return;
        }

        try
        {
            HCaptchaVerifyResponse result = await captchaProvider.VerifyAsync(
                tokens[0]!, context.Connection.RemoteIpAddress?.ToString(),
                context.RequestAborted);
            if (!result.Success)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "hCaptcha verification failed.");
            }
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex) when (ex is HCaptchaApiException or HttpRequestException ||
            (ex is OperationCanceledException && !context.RequestAborted.IsCancellationRequested))
        {
            Fail("hCaptcha verification was unavailable.");
        }
    }
}
