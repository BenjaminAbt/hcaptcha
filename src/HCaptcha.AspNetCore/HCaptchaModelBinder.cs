// Copyright © Benjamin Abt 2020-2024, all rights reserved

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>
/// A model binder that binds an hCaptcha verification result to an action parameter.
/// </summary>
public class HCaptchaModelBinder : IModelBinder
{
    private readonly IHCaptchaProvider _captchaProvider;
    private readonly HCaptchaOptions _captchaOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="HCaptchaModelBinder"/> class.
    /// </summary>
    /// <param name="captchaProvider">The hCaptcha provider used to verify the captcha token.</param>
    /// <param name="captchaOptionsAccessor">The configuration options for hCaptcha.</param>
    public HCaptchaModelBinder(IHCaptchaProvider captchaProvider, IOptions<HCaptchaOptions> captchaOptionsAccessor)
    {
        _captchaProvider = captchaProvider;
        _captchaOptions = captchaOptionsAccessor.Value;
    }

    /// <summary>
    /// Binds the model by verifying the hCaptcha token from the HTTP request.
    /// </summary>
    /// <param name="bindingContext">The <see cref="ModelBindingContext"/> that contains the HTTP request data to bind.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="bindingContext"/> is <c>null</c>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the HTTP request method is not "POST", as hCaptcha validation is only allowed on POST requests.
    /// </exception>
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        // Ensure bindingContext is not null
        ArgumentNullException.ThrowIfNull(bindingContext);

        // Validate that the request method is POST
        HttpContext httpContext = bindingContext.HttpContext;
        if (httpContext.Request.Method != "POST")
        {
            throw new InvalidOperationException($"{nameof(HCaptchaModelBinder)} can only be used with HTTP Post.");
        }

        // Retrieve the token from the form data
        StringValues token = httpContext.Request.Form[_captchaOptions.HttpPostResponseKeyName];

        // Verify the token with the hCaptcha provider
        try
        {
            HCaptchaVerifyResponse? result = await _captchaProvider
                .Verify(token, httpContext.Connection?.RemoteIpAddress?.ToString()).ConfigureAwait(false);
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (HCaptchaApiException apiException)
        {
            // If verification fails, add a model error and mark the binding result as failed
            bindingContext.ModelState.TryAddModelError(bindingContext.FieldName, apiException.Message);
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
