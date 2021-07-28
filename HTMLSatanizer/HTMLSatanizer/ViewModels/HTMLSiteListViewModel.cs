using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.ViewModels
{
    public class HTMLSiteListViewModel
    {
        public IEnumerable<HTMLSiteViewModel> HTMLs { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int SitesCount { get; set; }

        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage => this.CurrentPage == this.PagesCount ? this.PagesCount : this.CurrentPage + 1;

        public string Search { get; set; }
    }
}
