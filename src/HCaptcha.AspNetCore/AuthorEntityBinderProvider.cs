// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace BenjaminAbt.HCaptcha.AspNetCore;

public class AuthorEntityBinderProvider : IModelBinderProvider
{
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
