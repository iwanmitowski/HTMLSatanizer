using HTMLSatanizer.Data;
using HTMLSatanizer.Models;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace HTMLSatanizer.Controllers
{
    public class HTMLController : Controller
    {
        //TODO:
        //Latest added pages
        //All the pages with pagination

        private readonly IHTMLServices htmlServices;
        private readonly ApplicationDbContext dbContext;
        private readonly IDataBaseServices dataBaseServices;

        public HTMLController(IHTMLServices htmlServices, ApplicationDbContext dbContext, IDataBaseServices dataBaseServices)
        {
            this.htmlServices = htmlServices;
            this.dbContext = dbContext;
            this.dataBaseServices = dataBaseServices;
        }

        public IActionResult URL()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> URL(SiteInputModel model)
        {
            model.HTML = await this.htmlServices.GetHTMLFromGivenPage(model.URL);

            if (!ModelState.IsValid || model.HTML == null)
            {
                return this.Content("ГРЕШКА");
            }

            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            Site site = await this.dbContext.Set<Site>().FirstOrDefaultAsync<Site>(x => x.URL == model.URL);

            if (site != null)
            {
                this.dataBaseServices.Update(site);
            }
            else
            {
                site = new Site()
                {
                    URL = model.URL,
                    HTML = model.HTML,
                    HTMLSatanized = model.SatanizedHTML,
                    CreatedOn = DateTime.UtcNow,
                    Type = "URL",
                };

                await this.dataBaseServices.Add(site);
            }

            await this.dbContext.SaveChangesAsync();

            return View(model);
        }

        public IActionResult RawHTML()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RawHTMLAsync(RawHTMLInputModel model)
        {
            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            if (!ModelState.IsValid || model.HTML == null)
            {
                return this.Content("ГРЕШКА");
            }

            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            Site site = new Site()
            {
                URL = "RawHTML Input",
                HTML = model.HTML,
                HTMLSatanized = model.SatanizedHTML,
                CreatedOn = DateTime.UtcNow,
                Type = "RawHTML",
            };

            await this.dataBaseServices.Add(site);
            await this.dbContext.SaveChangesAsync();

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

            if (!ModelState.IsValid || model.HTML == null)
            {
                return this.Content("ГРЕШКА");
            }

            model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

            Site site = new Site()
            {
                URL = model.NameOnly,
                HTML = model.HTML,
                HTMLSatanized = model.SatanizedHTML,
                CreatedOn = DateTime.UtcNow,
                Type = "FromFile",
            };

            await this.dataBaseServices.Add(site);
            await this.dbContext.SaveChangesAsync();

            return View(model);
        }
    }
}
