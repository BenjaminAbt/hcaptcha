// Copyright Â© BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// Verifies hCaptcha challenge tokens.
/// </summary>
public interface IHCaptchaProvider
{
    /// <summary>
    /// Submits a token for server-side verification.
    /// </summary>
    /// <param name="token">The client's token.</param>
    /// <param name="remoteIp">Optional the client's IP address</param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="HCaptchaVerifyResponse"/></returns>
    /// <exception cref="HCaptchaApiException">if request failed.</exception>
    Task<HCaptchaVerifyResponse> VerifyAsync(string token, string? remoteIp = null,
        CancellationToken cancellationToken = default);
}
