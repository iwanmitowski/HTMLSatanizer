using Ganss.XSS;
using HTMLSatanizer.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services
{
    public class HTMLServices : IHTMLServices
    {
        private const string httpConstant = "http://";
        private const string httpsConstant = "https://";
        private readonly HttpClient client;

        public HTMLServices(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetHTMLFromGivenPage(string url)
        {
            url = url.StartsWith(httpsConstant) == false ? $"{httpConstant}{url}" : url;

            HttpResponseMessage response = await this.client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            return html;
        }

        public string SatanizeHTML(string html)
        {
            var sanitizer = new HtmlSanitizer();

            return WebUtility.HtmlDecode(Regex.Replace(sanitizer.Sanitize(html).Trim(), @"<[^>]+>", "&#128520;"));
        }
    }
}
