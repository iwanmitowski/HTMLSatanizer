using HTMLSatanizer.Data;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HTMLSatanizer.Models;
using System.Threading.Tasks;

namespace HTMLSatanizer.Controllers
{
    public class HTMLController : Controller
    {
        //TODO:
        //Migrations!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Latest added pages
        //All the pages with pagination
        
        private readonly IHTMLServices htmlServices;
        private readonly ApplicationDbContext dbContext;

        public HTMLController(IHTMLServices htmlServices, ApplicationDbContext dbContext)
        {
            this.htmlServices = htmlServices;
            this.dbContext = dbContext;
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

            var site = new Site() { URL = model.URL, HTML = model.SatanizedHTML };

            //Migrations!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //await dbContext.AddAsync(model);
            //await dbContext.SaveChangesAsync();

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
            //Migrations!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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
            //Migrations!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            return View(model);
        }
    }
}
