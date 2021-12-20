// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System;
using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.AspNetCore;

public static class HCaptchaModelBinderExtensions
{
    public static MvcOptions AddHCaptchaModelBinder(this MvcOptions mvcOptions,
        Action<HCaptchaModelBinderOptions>? captchaModelBinderOptions = null)
    {
        HCaptchaModelBinderOptions cmbo = new();
        captchaModelBinderOptions?.Invoke(cmbo);

        mvcOptions.ModelBinderProviders.Insert(cmbo.BinderPosition, new AuthorEntityBinderProvider());

        return mvcOptions;
    }
}
