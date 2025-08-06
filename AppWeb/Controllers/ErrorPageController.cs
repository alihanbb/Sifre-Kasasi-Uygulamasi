using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class ErrorPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
