using Microsoft.AspNetCore.Mvc;

namespace HTMLSatanizer.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult HttpError(int statusCode)
        {
            if (statusCode < 100 || statusCode > 599)
            {
                statusCode = 404;
            }

            return View(statusCode);
        }
    }
}
