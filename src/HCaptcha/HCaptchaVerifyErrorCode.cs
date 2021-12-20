// Copyright © Benjamin Abt 2020-2021, all rights reserved

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// Error Codes of hCaptcha response
/// </summary>
/// <remarks>https://docs.hcaptcha.com/#server</remarks>
public enum HCaptchaVerifyErrorCode
{
    /// <summary>
    /// given error code was unknown
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    Unknown = 0,
    /// <summary>
    /// missing-input-secret
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    MissingInputSecret,
    /// <summary>
    /// invalid-input-secret
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    InvalidInputSecret,
    /// <summary>
    /// missing-input-response
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    MissingInputResponse,
    /// <summary>
    /// invalid-input-response
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    InvalidInputRespose,
    /// <summary>
    /// bad request
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    BadRequest
}
