using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HTMLSatanizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDataBaseServices dataBaseServices;

        public HomeController(IDataBaseServices dataBaseServices)
        {
            this.dataBaseServices = dataBaseServices;
        }

        public IActionResult Index()
        {
            var elements = dataBaseServices.GetAll().OrderByDescending(x => x.CreatedOn).ThenBy(x => x.ModifiedOn).Take(4);

            List<HTMLSiteViewModel> models = new List<HTMLSiteViewModel>();

            foreach (var element in elements)
            {
                models.Add(
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

            return View(models);
        }
    }
}
