// Copyright Â© BEN ABT (https://benjamin-abt.com) - all rights reserved

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
    InvalidInputResponse,
    /// <summary>
    /// bad request
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    BadRequest,
    /// <summary>The submitted token expired.</summary>
    ExpiredInputResponse,
    /// <summary>The token has already been verified.</summary>
    AlreadySeenResponse,
    /// <summary>The remote IP is missing.</summary>
    MissingRemoteIp,
    /// <summary>The remote IP is invalid.</summary>
    InvalidRemoteIp,
    /// <summary>The test credentials do not match.</summary>
    NotUsingDummyPasscode,
    /// <summary>The site key does not belong to the supplied secret.</summary>
    SiteKeySecretMismatch
}
