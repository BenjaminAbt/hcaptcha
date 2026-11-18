// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>
/// Extension methods for configuring the hCaptcha model binder in MVC options.
/// </summary>
public static class HCaptchaModelBinderExtensions
{
    /// <summary>
    /// Adds the hCaptcha model binder to the <see cref="MvcOptions"/> with optional configuration.
    /// </summary>
    /// <param name="mvcOptions">The <see cref="MvcOptions"/> to which the model binder will be added.</param>
    /// <param name="captchaModelBinderOptions">Optional binder configuration.</param>
    /// <returns>The updated <see cref="MvcOptions"/> with the hCaptcha model binder added.</returns>
    public static MvcOptions AddHCaptchaModelBinder(this MvcOptions mvcOptions,
        Action<HCaptchaModelBinderOptions>? captchaModelBinderOptions = null)
    {
        // Create a default set of model binder options
        HCaptchaModelBinderOptions cmbo = new();

        // Apply custom configuration if provided
        captchaModelBinderOptions?.Invoke(cmbo);

        // Insert the custom HCaptchaModelBinderProvider at the specified position
        mvcOptions.ModelBinderProviders.Insert(cmbo.BinderPosition, new HCaptchaModelBinderProvider());

        return mvcOptions;
    }
}
