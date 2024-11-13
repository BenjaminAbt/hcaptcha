// Copyright © Benjamin Abt 2020-2024, all rights reserved

using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.AspNetCore;

/// <summary>
/// Options for configuring the position of the hCaptcha model binder in the model binder provider list.
/// </summary>
public class HCaptchaModelBinderOptions
{
    /// <summary>
    /// Gets or sets the position of the hCaptcha model binder in the model binder provider list.
    /// </summary>
    /// <remarks>
    /// The binder is inserted at the specified position in the <see cref="MvcOptions.ModelBinderProviders"/> list.
    /// A value of 0 means it will be inserted at the beginning of the list.
    /// </remarks>
    public int BinderPosition { get; set; } = 0;
}
