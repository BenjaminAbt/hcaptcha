using System.Collections.Generic;

namespace BenjaminAbt.HCaptcha
{
    /// <summary>
    /// hCaptcha Response Extensions
    /// </summary>
    public static class HCaptchaResponseExtensions
    {
        /// <summary>
        /// Directory of official documenated error codes to enum values
        /// </summary>
        /// <remarks>https://docs.hcaptcha.com/#server</remarks>
        public static IDictionary<string, HCaptchaVerifyErrorCode> ErrorCodeDictionary = new Dictionary<string, HCaptchaVerifyErrorCode>()
        {
            { "missing-input-secret", HCaptchaVerifyErrorCode.MissingInputSecret },
            { "invalid-input-secret", HCaptchaVerifyErrorCode.InvalidInputSecret },

            { "missing-input-response",HCaptchaVerifyErrorCode.MissingInputResponse },
            { "invalid-input-response",HCaptchaVerifyErrorCode.InvalidInputRespose },

            { "bad-request", HCaptchaVerifyErrorCode.BadRequest }
        };

        /// <summary>
        /// Maps the string-based error codes to enums. Uses <see cref="ErrorCodeDictionary"/> as mapping table.
        /// </summary>
        /// <remarks>https://docs.hcaptcha.com/#server</remarks>
        public static IEnumerable<HCaptchaVerifyErrorCode> GetErrorCodes(this HCaptchaVerifyResponse response)
        {
            if (response?.ErrorCodes is null) yield break;

            foreach (string e in response.ErrorCodes)
            {
                if (ErrorCodeDictionary.TryGetValue(e, out HCaptchaVerifyErrorCode errorCode))
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
}