// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace BenjaminAbt.HCaptcha;

/// <summary>Submits tokens to the hCaptcha verification endpoint.</summary>
public sealed class HCaptchaProvider(HttpClient client, IOptions<HCaptchaOptions> options) : IHCaptchaProvider
{
    /// <inheritdoc />
    public async Task<HCaptchaVerifyResponse> VerifyAsync(string token, string? remoteIp = null,
        CancellationToken cancellationToken = default)
    {
        HCaptchaOptions settings = options.Value;
        Dictionary<string, string> fields = new()
        {
            ["secret"] = settings.Secret,
            ["response"] = token,
            ["sitekey"] = settings.SiteKey
        };
        if (settings.VerifyRemoteIp && !string.IsNullOrWhiteSpace(remoteIp))
        {
            fields.Add("remoteip", remoteIp);
        }

        using FormUrlEncodedContent body = new(fields);
        using HttpResponseMessage result = await client.PostAsync("/siteverify", body, cancellationToken)
            .ConfigureAwait(false);
        try
        {
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadFromJsonAsync(
                HCaptchaJsonContext.Default.HCaptchaVerifyResponse, cancellationToken)
                .ConfigureAwait(false) ?? throw new JsonException("Expected a verification response.");
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException)
        {
            throw new HCaptchaApiException(result.StatusCode, ex);
        }
    }
}

[JsonSerializable(typeof(HCaptchaVerifyResponse))]
internal partial class HCaptchaJsonContext : JsonSerializerContext;
