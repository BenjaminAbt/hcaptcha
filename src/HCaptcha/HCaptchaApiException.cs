// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Net;

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// Represents an exception that occurs when an hCaptcha API request fails.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="HCaptchaApiException"/> class.
/// </remarks>
/// <param name="statusCode">The HTTP status code returned by the hCaptcha API.</param>
/// <param name="apiException">The inner exception that contains details about the API error.</param>
public class HCaptchaApiException(HttpStatusCode statusCode, Exception apiException)
    : Exception("hCaptcha API request failed. See inner exception for details.", apiException)
{
    /// <summary>
    /// Gets the HTTP status code associated with the failed hCaptcha API request.
    /// </summary>
    public HttpStatusCode StatusCode { get; } = statusCode;
}
