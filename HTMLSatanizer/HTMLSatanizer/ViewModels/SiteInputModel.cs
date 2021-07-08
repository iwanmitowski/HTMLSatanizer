using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.ViewModels
{
    public class SiteInputModel
    {
        [RegularExpression(@"[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)")]
        //[RegularExpression(@"^[(ht|f)tp(s?)\:\/\/]?[0-9a-zA-Z]([-.\w]*[a-яA-Я0-9])*(:(0-9)*)*(\/?)([a-яA-Я0-9\-\.\?\,\'\/\\\+&%\$#_]*)?$")]
        public string URL { get; set; }

        public string HTML { get; set; }

        public string SatanizedHTML { get; set; }
    }
}
