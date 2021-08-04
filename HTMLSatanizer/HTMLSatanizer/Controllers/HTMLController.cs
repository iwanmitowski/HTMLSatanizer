using HTMLSatanizer.Data;
using HTMLSatanizer.EmailSender.Contracts;
using HTMLSatanizer.Models;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSatanizer.Controllers
{
    public class HTMLController : Controller
    {
        private const int ItemsPerPage = 6;

        private readonly IHTMLServices htmlServices;
        private readonly ApplicationDbContext dbContext;
        private readonly IDataBaseServices dataBaseServices;
        private readonly IEmailSender emailSender;

        public HTMLController(
            IHTMLServices htmlServices,
            ApplicationDbContext dbContext,
            IDataBaseServices dataBaseServices,
            IEmailSender emailSender)
        {
            this.htmlServices = htmlServices;
            this.dbContext = dbContext;
            this.dataBaseServices = dataBaseServices;
            this.emailSender = emailSender;
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

                    site.RecentUpdate = site.CreatedOn;

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

            site.RecentUpdate = site.CreatedOn;

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

                site.RecentUpdate = site.CreatedOn;

                await this.dataBaseServices.Add(site);
                await this.dbContext.SaveChangesAsync();
            }

            return View(model);
        }

        public IActionResult ById(int id)
        {
            var element = dataBaseServices.GetById(id);

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
            var pagesCount = (int)Math.Ceiling(sitesCount / (decimal)ItemsPerPage);

            if (id < 0 || id > pagesCount)
            {
                return NotFound();
            }

            var query = all
                .OrderByDescending(x => x.RecentUpdate)
                .Skip(skip)
                .Take(ItemsPerPage)
                .ToList();

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

        [HttpPost]
        public async Task<IActionResult> SendToEmail(int id)
        {
            var element = dataBaseServices.GetById(id);
            var email = HttpContext.Request.Form["email"].ToString();

            if (!ModelState.IsValid || email == string.Empty || element == null)
            {
                return this.Redirect("/Error/HttpError?statusCode=502");
            }

            string content = $"<h1>Satanized content: {element.URL}</h1>";

            StringBuilder html = new StringBuilder();

            html.AppendLine(content);
            html.AppendLine($"<h3>Created On: {element.CreatedOn}</h3>");
            html.AppendLine(@$"<h3>Modified On: {(element.ModifiedOn == null ? "Never" : element.ModifiedOn)}</h3>");
            html.AppendLine();
            html.AppendLine(element.HTMLSatanized);

            await emailSender.SendEmailAsync(
                "iwan.mitowski@gmail.com",
                "HTMLSatanizer",
                email,
                content,
                html.ToString());

            return View();
        }
    }
}
