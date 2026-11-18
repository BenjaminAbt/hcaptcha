// Copyright Â© BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Collections.Frozen;

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// hCaptcha Response Extensions
/// </summary>
public static class HCaptchaResponseExtensions
{
    /// <summary>
    /// Directory of official documented error codes to enum values
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    private static readonly FrozenDictionary<string, HCaptchaVerifyErrorCode> s_errorCodes =
        new Dictionary<string, HCaptchaVerifyErrorCode>()
        {
            { "missing-input-secret", HCaptchaVerifyErrorCode.MissingInputSecret },
            { "invalid-input-secret", HCaptchaVerifyErrorCode.InvalidInputSecret },

            { "missing-input-response", HCaptchaVerifyErrorCode.MissingInputResponse },
            { "invalid-input-response", HCaptchaVerifyErrorCode.InvalidInputResponse },

            { "expired-input-response", HCaptchaVerifyErrorCode.ExpiredInputResponse },
            { "already-seen-response", HCaptchaVerifyErrorCode.AlreadySeenResponse },
            { "missing-remoteip", HCaptchaVerifyErrorCode.MissingRemoteIp },
            { "invalid-remoteip", HCaptchaVerifyErrorCode.InvalidRemoteIp },
            { "not-using-dummy-passcode", HCaptchaVerifyErrorCode.NotUsingDummyPasscode },
            { "sitekey-secret-mismatch", HCaptchaVerifyErrorCode.SiteKeySecretMismatch },
            { "bad-request", HCaptchaVerifyErrorCode.BadRequest }
        }.ToFrozenDictionary(StringComparer.Ordinal);

    /// <summary>
    /// Maps the string-based error codes to enums.
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/#server</remarks>
    public static IEnumerable<HCaptchaVerifyErrorCode> GetErrorCodes(this HCaptchaVerifyResponse response)
    {
        if (response?.ErrorCodes is null)
        {
            yield break;
        }

        foreach (string e in response.ErrorCodes)
        {
            if (s_errorCodes.TryGetValue(e, out HCaptchaVerifyErrorCode errorCode))
            {
                yield return errorCode;
            }
            else
            {
                yield return HCaptchaVerifyErrorCode.Unknown;
            }
        }
    }
}
