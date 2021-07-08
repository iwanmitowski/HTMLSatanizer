using HTMLSatanizer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services
{
    public class HTMLServices : IHTMLServices
    {   
        public string SatanizeHTML(string html)
        {
            return WebUtility.HtmlDecode(Regex.Replace(html, @"<[^>]+>", "&#128520;"));
        }
    }
}
