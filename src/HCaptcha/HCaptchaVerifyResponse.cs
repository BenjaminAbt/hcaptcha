using System;
using Newtonsoft.Json;

namespace BenjaminAbt.HCaptcha
{
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
        [JsonProperty("success")]
        public bool Success { get; set; }

        /// <summary>
        /// timestamp of the captcha (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        /// </summary>
        /// <remarks>https://docs.hcaptcha.com/#server</remarks>
        [JsonProperty("challenge_ts")]
        public DateTimeOffset Timestamp { get; set; }

        /// <summary>
        /// the hostname of the site where the captcha was solved
        /// </summary>
        /// <remarks>https://docs.hcaptcha.com/#server</remarks>
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// optional: whether the response will be credited
        /// </summary>
        /// <remarks>https://docs.hcaptcha.com/#server</remarks>
        [JsonProperty("credit")]
        public bool Credit { get; set; }

        /// <summary>
        /// string based error code array
        /// </summary>
        /// <remarks>https://docs.hcaptcha.com/#server</remarks>
        [JsonProperty("error-codes")]
        public string[] ErrorCodes { get; set; }
    }
}