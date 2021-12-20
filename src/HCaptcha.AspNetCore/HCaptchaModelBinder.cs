// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace BenjaminAbt.HCaptcha.AspNetCore;

public class HCaptchaModelBinder : IModelBinder
{
    private readonly IHCaptchaProvider _captchaProvider;
    private readonly HCaptchaOptions _captchaOptions;

    public HCaptchaModelBinder(IHCaptchaProvider captchaProvider, IOptions<HCaptchaOptions> captchaOptionsAccessor)
    {
        _captchaProvider = captchaProvider;
        _captchaOptions = captchaOptionsAccessor.Value;
    }

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        // validate context
        var httpContext = bindingContext.HttpContext;
        if (httpContext.Request.Method != "POST")
        {
            throw new InvalidOperationException($"{nameof(HCaptchaModelBinder)} can only be used with HTTP Post.");
        }

        // read token
        var token = httpContext.Request.Form[_captchaOptions.HttpPostResponseKeyName];

        // verify
        try
        {
            var result = await _captchaProvider.Verify(token, httpContext.Connection?.RemoteIpAddress?.ToString());
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (HCaptchaApiException apiException)
        {
            bindingContext.ModelState.TryAddModelError(bindingContext.FieldName, apiException.Message);
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}
