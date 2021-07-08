using HTMLSatanizer.Services;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HTMLSatanizer.Controllers
{
    public class HTMLController : Controller
    {
        //TODO:
        //Custom error pages!
        private readonly IHTMLServices htmlServices;

        public HTMLController(IHTMLServices htmlServices)
        {
            this.htmlServices = htmlServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SiteInputModel model)
        {
            model.HTML = await this.htmlServices.GetHTMLFromGivenPage(model.URL);
            
            if (model.HTML==null)
            {
                return this.Content("ГРЕШКА");
            }

            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            return View(model);
        }
    }
}
