// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>
/// Provides a model binder for binding an <see cref="HCaptchaVerifyResponse"/> to an action parameter.
/// </summary>
public class HCaptchaModelBinderProvider : IModelBinderProvider
{
    /// <summary>
    /// Returns a model binder for the <see cref="HCaptchaVerifyResponse"/> model type, if applicable.
    /// </summary>
    /// <param name="context">The context that provides information about the model being bound.</param>
    /// <returns>
    /// A binder for <see cref="HCaptchaVerifyResponse"/>, or <c>null</c> for other types.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="context"/> is <c>null</c>.
    /// </exception>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (context.Metadata.ModelType == typeof(HCaptchaVerifyResponse))
        {
            return new BinderTypeModelBinder(typeof(HCaptchaModelBinder));
        }
        return null;
    }
}
