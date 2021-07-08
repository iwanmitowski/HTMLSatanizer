using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.ViewModels
{
    public class SiteInputModel
    {
        public string URL { get; set; }

        public string HTML { get; set; }

        public string SatanizedHTML { get; set; }
    }
}
