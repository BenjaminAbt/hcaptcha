// Copyright © Benjamin Abt 2020-2021, all rights reserved

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Refit;

namespace BenjaminAbt.HCaptcha;

/// <summary>
/// HCaptcha Provider - API communication
/// </summary>
public class HCaptchaProvider : IHCaptchaProvider
{
    private readonly IHCaptchaApi _captchaApi;
    private readonly HCaptchaOptions _captchaOptions;

    /// <summary>
    /// Creates an instance of <see cref="HCaptchaProvider"/>
    /// </summary>
    public HCaptchaProvider(IHCaptchaApi captchaApi, IOptions<HCaptchaOptions> captchaOptionsAccessor)
    {
        _captchaApi = captchaApi;
        _captchaOptions = captchaOptionsAccessor.Value;
    }

    /// <summary>
    /// Requests the hCaptcha API via the provided <see cref="IHCaptchaApi"/>. Timeout configuration is provided via <paramref name="cancellationToken"/>
    /// </summary>
    /// <param name="token">The client's token.</param>
    /// <param name="remoteIp">Optional the client's IP address</param>
    /// <param name="cancellationToken"></param>
    /// <returns><see cref="HCaptchaVerifyResponse"/></returns>
    /// <exception cref="HCaptchaApiException">if request failed.</exception>
    public async Task<HCaptchaVerifyResponse?> Verify(string token, string? remoteIp = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await _captchaApi.Verify(_captchaOptions.Secret, token,
                _captchaOptions.VerifyRemoteIp ? remoteIp : null, cancellationToken).ConfigureAwait(false);
        }
        catch (ApiException e)
        {
            throw new HCaptchaApiException(e.StatusCode, e);
        }
    }
}
