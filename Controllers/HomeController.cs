using Microsoft.AspNetCore.Mvc;

namespace boardroombooking1.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
