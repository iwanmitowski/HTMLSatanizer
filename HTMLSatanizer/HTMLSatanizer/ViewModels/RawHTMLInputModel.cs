using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.ViewModels
{
    public class RawHTMLInputModel
    {
        [Required]
        public string HTML { get; set; }

        public string SatanizedHTML { get; set; }

    }
}
