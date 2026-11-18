# hCaptcha for .NET

<p align="center">
    <a href="https://www.nuget.org/packages/HCaptcha/"><img src="https://img.shields.io/nuget/v/HCaptcha?label=HCaptcha&logo=nuget&color=0f766e" alt="HCaptcha on NuGet" /></a>
    <a href="https://www.nuget.org/packages/HCaptcha.AspNetCore/"><img src="https://img.shields.io/nuget/v/HCaptcha.AspNetCore?label=HCaptcha.AspNetCore&logo=nuget&color=0369a1" alt="HCaptcha.AspNetCore on NuGet" /></a>
    <a href="https://github.com/BenjaminAbt/hcaptcha/blob/main/LICENSE"><img src="https://img.shields.io/badge/License-MIT-f4b400.svg" alt="MIT license" /></a>
</p>

<p align="center">
    <strong>Server-side hCaptcha verification for .NET 8, 9, 10, and 11.</strong>
    <br />
    A typed HTTP client for .NET and a model binder for ASP.NET Core MVC.
</p>

---

The hCaptcha widget gives the browser a single-use token. Your application sends that token to hCaptcha and checks the response before processing the form. This unofficial client handles verification and makes the result available to your application as an `HCaptchaVerifyResponse`.

| NuGet package | Use it for |
| --- | --- |
| [HCaptcha](https://www.nuget.org/packages/HCaptcha/) | Verify a token directly from a .NET application. |
| [HCaptcha.AspNetCore](https://www.nuget.org/packages/HCaptcha.AspNetCore/) | Register the client with dependency injection and bind the verified response to an MVC action parameter. Includes the core package. |

## Table of Contents

- [Usage with ASP.NET Core](#usage-with-aspnet-core)
- [Direct verification](#direct-verification)
- [Sample and build](#sample-and-build)
- [Releases](#releases)

## Usage with ASP.NET Core

```sh
dotnet add package HCaptcha.AspNetCore
```

Configure the public site key with `HCaptcha:SiteKey` and supply the server secret through your secret store or the `HCaptcha__Secret` environment variable. Register the client and the model binder in `Program.cs`:

```csharp
using BenjaminAbt.HCaptcha.AspNetCore;

builder.Services.AddHCaptcha(builder.Configuration.GetRequiredSection("HCaptcha"));
builder.Services.AddControllersWithViews(options => options.AddHCaptchaModelBinder());
```

In a Razor form, render the widget with the public site key:

```cshtml
@inject Microsoft.Extensions.Options.IOptions<BenjaminAbt.HCaptcha.HCaptchaOptions> Captcha
<form method="post">
    @Html.AntiForgeryToken()
    <div class="h-captcha" data-sitekey="@Captcha.Value.SiteKey"></div>
    <button type="submit">Submit</button>
</form>
<script src="@Captcha.Value.JavaScriptUrl" async defer></script>
```

Check the server result before executing the protected operation:

```csharp
[HttpPost, ValidateAntiForgeryToken]
public IActionResult Submit(HCaptchaVerifyResponse? captcha)
{
    if (!ModelState.IsValid || captcha?.Success != true)
        return BadRequest("Please complete a new challenge.");

    return Ok();
}
```

The binder reads one token from the configured form field (default `h-captcha-response`) and forwards request cancellation. The client posts a URL-encoded form to `https://api.hcaptcha.com/siteverify`, including the expected site key. By default it includes the client IP when available. Behind a reverse proxy, configure ASP.NET Core forwarded headers for your trusted proxies. Set `HCaptcha:VerifyRemoteIp` to `false` to omit the IP. Tokens are single-use. Obtain a fresh token after a failed or timed-out verification. [hCaptcha developer guide](https://docs.hcaptcha.com/#verify-the-user-response-server-side)

## Direct verification

Inject `IHCaptchaProvider` and call `await provider.VerifyAsync(token, remoteIp, cancellationToken)`. Authorize the operation only when the returned response has `Success == true`. For standalone use, construct `HCaptchaProvider` with an `HttpClient` whose base address is the verification API and `IOptions<HCaptchaOptions>` containing the site key and secret. Direct callers should handle `HCaptchaApiException` for HTTP status or malformed JSON, `HttpRequestException` for transport failures and cancellation or timeouts.

## Sample and build

The [MVC sample](sample/sample.aspnetcore) targets .NET 11. Its Development configuration uses hCaptcha's public, always-pass test credentials. Supply your own credentials for a deployed application. [hCaptcha test keys](https://docs.hcaptcha.com/#integration-testing-test-keys)

```sh
dotnet run --project sample/sample.aspnetcore
dotnet build -c Release
dotnet test -c Release --no-build --coverlet --coverlet-include '[BenjaminAbt.HCaptcha*]*' --coverlet-output-format cobertura --coverlet-threshold 90 --coverlet-threshold-type line --coverlet-threshold-stat Minimum
dotnet pack -c Release --no-build -o artifacts
```

Build with SDK 11.0.100 or a compatible patch. The xUnit tests run on Microsoft.Testing.Platform. Install the ASP.NET Core runtimes for .NET 8, 9, 10, and 11 to run the full suite.

The `HCaptcha.UnitTests` project measures both shipping libraries with Coverlet. CI requires at least 90% line coverage for each library on each target framework. Pull requests publish the Cobertura reports alongside the TRX test results.

## Releases

Pull requests build, test all four frameworks, and check packaging. A successful main build creates a GitHub draft release for its Nerdbank.GitVersioning version and attaches the two NuGet packages and their symbols. Review and publish the desired draft to distribute those exact assets.

Configure NuGet.org Trusted Publishing for the package owner, this repository and workflow `release-publish.yml`. Set the GitHub Actions secret `NUGET_USER` to the NuGet.org account name. The workflow exchanges GitHub's OIDC token for a short-lived publishing key.

## Donation

Please donate, if possible, to an institution of your choice such as child cancer aid or a children's hospice.

## License

[MIT License](LICENSE)
