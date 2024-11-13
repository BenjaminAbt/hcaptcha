// Copyright © Benjamin Abt 2020-2024, all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>
/// Provides a model binder for binding an <see cref="HCaptchaVerifyResponse"/> to an action parameter.
/// </summary>
public class AuthorEntityBinderProvider : IModelBinderProvider
{
    /// <summary>
    /// Returns a model binder for the <see cref="HCaptchaVerifyResponse"/> model type, if applicable.
    /// </summary>
    /// <param name="context">The context that provides information about the model being bound.</param>
    /// <returns>
    /// An <see cref="IModelBinder"/> instance if the model type is <see cref="HCaptchaVerifyResponse"/>, otherwise <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="context"/> is <c>null</c>.
    /// </exception>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        // Ensure the context is not null
        ArgumentNullException.ThrowIfNull(context);

        // Check if the model type is HCaptchaVerifyResponse
        if (context.Metadata.ModelType == typeof(HCaptchaVerifyResponse))
        {
            // Return a binder for HCaptchaVerifyResponse
            return new BinderTypeModelBinder(typeof(HCaptchaModelBinder));
        }

        // Return null if no binder is required for the model type
        return null;
    }
}
