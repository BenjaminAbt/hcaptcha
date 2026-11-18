// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore.Controllers;

public class HomeController : Controller
{
    [HttpGet, Route("")]
    public IActionResult Index()
    {
        return View(new IndexViewModel());
    }

    [HttpPost, Route(""), ValidateAntiForgeryToken]
    public IActionResult Index(HCaptchaVerifyResponse? hCaptcha)
    {
        return ModelState.IsValid && hCaptcha?.Success == true
            ? View(new IndexViewModel(hCaptcha))
            : BadRequest("Challenge verification failed. Please try again.");
    }
}

public class IndexViewModel(HCaptchaVerifyResponse? response = null)
{
    public HCaptchaVerifyResponse? Response { get; } = response;
}
