// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System;
using System.Net;
using Refit;

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// hCapcha API Exception
/// </summary>
/// <remarks>Inner exception is a of type <see cref="ApiException"/> which is a part of Refit.</remarks>
public class HCaptchaApiException : Exception
{
    /// <summary>
    /// Status Code
    /// </summary>
    public HttpStatusCode StatusCode { get; } // Refit as Inner Exception

    /// <summary>
    /// Creates an instance of <see cref="HCaptchaApiException"/> with wrapped <see cref="ApiException"/>
    /// </summary>
    public HCaptchaApiException(HttpStatusCode statusCode, ApiException apiException)
        : base("hCaptcha API request failed. See inner exception for details.", apiException)
    {
        StatusCode = statusCode;
    }
}
