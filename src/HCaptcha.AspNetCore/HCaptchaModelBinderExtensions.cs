using System;
using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.AspNetCore
{
    public static class HCaptchaModelBinderExtensions
    {
        public static MvcOptions AddHCaptchaModelBinder(this MvcOptions mvcOptions, Action<HCaptchaModelBinderOptions> captchaModelBinderOptions = null)
        {
            HCaptchaModelBinderOptions cmbo = new HCaptchaModelBinderOptions();
            captchaModelBinderOptions?.Invoke(cmbo);

            mvcOptions.ModelBinderProviders.Insert(cmbo.BinderPosition, new AuthorEntityBinderProvider());

            return mvcOptions;
        }
    }
}