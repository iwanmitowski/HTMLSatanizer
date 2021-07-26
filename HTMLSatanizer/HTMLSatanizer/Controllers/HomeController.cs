using Microsoft.AspNetCore.Mvc;

namespace HTMLSatanizer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
