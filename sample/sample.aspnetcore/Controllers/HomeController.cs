// Copyright © Benjamin Abt 2020-2021, all rights reserved

using Microsoft.AspNetCore.Mvc;

namespace BenjaminAbt.HCaptcha.Samples.AspNetCore.Controllers;

public class HomeController : Controller
{
    [HttpGet, Route("")]
    public IActionResult Index() => View(new IndexViewModel());

    [HttpPost, Route("")]
    public IActionResult Index(HCaptchaVerifyResponse hCaptcha) => View(new IndexViewModel(hCaptcha));
}

public class IndexViewModel
{
    public HCaptchaVerifyResponse? Response { get; }

    public IndexViewModel(HCaptchaVerifyResponse? response = null) => Response = response;
}
