// Copyright © Benjamin Abt 2020-2024, all rights reserved

using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore.Controllers;

public class HomeController : Controller
{
    [HttpGet, Route("")]
    public IActionResult Index() => View(new IndexViewModel());

    [HttpPost, Route("")]
    public IActionResult Index(HCaptchaVerifyResponse hCaptcha) => View(new IndexViewModel(hCaptcha));
}

public class IndexViewModel(HCaptchaVerifyResponse? response = null)
{
    public HCaptchaVerifyResponse? Response { get; } = response;
}
