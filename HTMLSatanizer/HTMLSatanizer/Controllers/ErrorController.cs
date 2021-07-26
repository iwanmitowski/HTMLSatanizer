using Microsoft.AspNetCore.Mvc;

namespace HTMLSatanizer.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult HttpError(int statusCode)
        {
            return View(statusCode);
        }
    }
}
