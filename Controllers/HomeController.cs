using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IdentityApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityApp.Controllers;

[Authorize] // ben burda diyorum ki kullanıcı girişi olmadan sayfadaki hiçbir işlemi yapamazsın yani bir cookie ye ihtiyaç duyar
/*
Authentication(kimlik doğrulama)
Cookie Based Authentication(Tarayıcılarda kullanılan-kullanıcı uygulamaya bir login,email veya parola bilgisiyle login işlemi gerçekleştirdiğinde kullanıcının tarayıcısına bir cookie(çerez bırakıyoruz) yani tarayıcıyı sonradan ziyaret ettiğinde kullanıcı hatırlanması için bazı bilgilerin tarıcı belleğinde saklanması. Bu şekilde tarayıcı kullanıcı tekrar giriş yapmadan istediği bilgileri ona sağlar
Token Based Authentication - JWT(üsteki işlemin diğer yöntemi geelde mobil uygulamalarda kullanıyor kullanıcıya benzersiz bir kimlik oluşturup kullanıcı girişi tekrardan olmadan girişi sağlıyor)
External Provider Authentication(bu yöntemde çok kullanılan bir yöntem burda kullanıcı oluşturmadan google,instagram veya facebook ile giriş yapmışsam o bilgilerle girişini otomatik sağlıyor)
*/

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
