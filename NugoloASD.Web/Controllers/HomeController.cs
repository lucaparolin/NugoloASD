using Microsoft.AspNetCore.Mvc;

namespace NugoloASD.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // Redirect alla dashboard
        return RedirectToAction("Index", "Dashboard");
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
