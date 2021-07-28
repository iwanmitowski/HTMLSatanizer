using HTMLSatanizer.Data;
using HTMLSatanizer.Models;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Controllers
{
    public class HTMLController : Controller
    {
        private const int ItemsPerPage = 6;

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

            if (!model.HTML.StartsWith("Error"))
            {
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
            }


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

            if (!model.HTML.StartsWith("Error"))
            {
                model.SatanizedHTML = this.htmlServices.SatanizeHTML(model.HTML);

                Site site = new Site()
                {
                    URL = model.NameOnly,
                    HTML = model.HTML,
                    HTMLSatanized = model.SatanizedHTML,
                    CreatedOn = DateTime.UtcNow,
                    Type = "File",
                };

                await this.dataBaseServices.Add(site);
                await this.dbContext.SaveChangesAsync();
            }

            return View(model);
        }

        public IActionResult ById(int Id)
        {
            var element = dataBaseServices.GetById(Id);

            if (!ModelState.IsValid || element == null)
            {
                return this.NotFound();
            }

            HTMLSiteViewModel model = new HTMLSiteViewModel()
            {
                Id = element.Id,
                URL = element.URL,
                Type = element.Type,
                HTML = element.HTML,
                HTMLSatanized = element.HTMLSatanized,
                CreatedOn = element.CreatedOn,
                ModifiedOn = element.ModifiedOn,
            };

            return View(model);
        }

        public IActionResult All(int id/*, string search*/)
        {
            //Setting up the elements
            id = Math.Max(1, id);
            var skip = (id - 1) * ItemsPerPage;
            var all = dataBaseServices.GetAll();
            var sitesCount = all.Count();

            if (id < 0 || id > sitesCount)
            {
                return NotFound();
            }

            var query = all
                .OrderByDescending(x => x.ModifiedOn)
                .ThenByDescending(x => x.CreatedOn)
                .Skip(skip)
                .Take(ItemsPerPage)
                .ToList();



            var pagesCount = (int)Math.Ceiling(sitesCount / (decimal)ItemsPerPage);


            List<HTMLSiteViewModel> sites = new List<HTMLSiteViewModel>();
            foreach (var element in query)
            {
                sites.Add(
                    new HTMLSiteViewModel()
                    {
                        Id = element.Id,
                        URL = element.URL,
                        Type = element.Type,
                        HTML = element.HTML,
                        HTMLSatanized = element.HTMLSatanized,
                        CreatedOn = element.CreatedOn,
                        ModifiedOn = element.ModifiedOn,
                    }
                );
            }

            var model = new HTMLSiteListViewModel()
            {
                HTMLs = sites,
                CurrentPage = id,
                PagesCount = pagesCount,
                SitesCount = sitesCount,
                Search = null,
            };

            return View(model);
        }
    }
}
