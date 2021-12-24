// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System;
using System.Text.Json.Serialization;

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// Response Model to get the verification result
/// </summary>
/// <remarks>https://docs.hcaptcha.com/#server</remarks>
public class HCaptchaVerifyResponse
{
    /// <summary>
    /// indicates if verify was successfully or not
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    /// <summary>
    /// timestamp of the captcha (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("challenge_ts")]
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// the hostname of the site where the captcha was solved
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("hostname")]
    public string? Hostname { get; set; }

    /// <summary>
    /// optional: whether the response will be credited
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("credit")]
    public bool Credit { get; set; }

    /// <summary>
    /// string based error code array
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("error-codes")]
    public string[]? ErrorCodes { get; set; }

    /// <summary>
    /// ENTERPRISE feature: a score denoting malicious activity
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("score")]
    public double? Score { get; set; }

    /// <summary>
    /// ENTERPRISE feature: reason(s) for score.
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    [JsonPropertyName("score_reason")]
    public string[]? ScoreReason { get; set; }
}
