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
        //Latest added pages
        //All the pages with pagination
        //AJAX For the submiting
        private readonly IHTMLServices htmlServices;

        public HTMLController(IHTMLServices htmlServices)
        {
            this.htmlServices = htmlServices;
        }

        public IActionResult URL()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> URL(SiteInputModel model)
        {
            model.HTML = await this.htmlServices.GetHTMLFromGivenPage(model.URL);

            if (model.HTML == null)
            {
                return this.Content("ГРЕШКА");
            }

            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            return View(model);
        }

        public IActionResult RawHTML()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RawHTML(RawHTMLInputModel model)
        {
            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            return View(model);
        }

        public IActionResult File()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> File(FileInputModel model)
        {
            model.HTML = await this.htmlServices.ReadTextFromFile(model.File);
            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            return View(model);
        }
    }
}
