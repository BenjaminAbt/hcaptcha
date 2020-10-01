namespace BenjaminAbt.HCaptcha
{
    /// <summary>
    /// hCaptcha Options
    /// </summary>
    /// <remarks>https://docs.hcaptcha.com/</remarks>
    public class HCaptchaOptions
    {
        /// <summary>
        /// hCaptcha Site Key
        /// </summary>
        public string SiteKey { get; set; } = "";

        /// <summary>
        /// hCaptcha Site Secret
        /// </summary>
        public string Secret { get; set; } = "";

        /// <summary>
        /// The HTTP Post Form Key to get the token from
        /// </summary>
        public string HttpPostResponseKeyName { get; set; } = "h-captcha-response";

        /// <summary>
        /// if true client IP is passed to hCaptcha token verification
        /// </summary>
        public bool VerifyRemoteIp { get; set; } = true;

        /// <summary>
        ///  Full Url to hCaptchy JavaScript
        /// </summary>
        public const string JavaScriptUrl = "https://hcaptcha.com/1/api.js";

        /// <summary>
        /// The hCaptcha base URL
        /// </summary>
        public const string ApiBaseUrl = "https://hcaptcha.com/";
    }
}
