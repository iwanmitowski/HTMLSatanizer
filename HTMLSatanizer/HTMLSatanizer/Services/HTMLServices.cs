using Ganss.XSS;
using HeyRed.Mime;
using HTMLSatanizer.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services
{
    public class HTMLServices : IHTMLServices
    {
        private const string httpsConstant = "https://";
        private const string errorMessageGlobal = "Error occured, try again later!";
        private const string errorMessageHTTPS = "Error occured! Please try something different! Ensure that the site is using HTTPS!";
        private const string errorMessageMimeType = "Unsupported file extension.";
        private readonly HttpClient client;

        public HTMLServices(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> GetHTMLFromGivenPage(string url)
        {
            url = url.StartsWith(httpsConstant) == false ? $"{httpsConstant}{url}" : url;
            string html;

            try
            {
                HttpResponseMessage response = await this.client.GetAsync(url);

                var responseAsBytes = await response.Content.ReadAsByteArrayAsync();

                html = Encoding.UTF8.GetString(responseAsBytes, 0, responseAsBytes.Length);
            }
            catch (Exception)
            {
                return errorMessageHTTPS;
            }

            return html;
        }

        public string SatanizeHTML(string html)
        {
            var sanitizer = new HtmlSanitizer();

            return WebUtility.HtmlDecode(Regex.Replace(sanitizer.Sanitize(html).Trim(), @"<[^>]+>", "&#128520;"));
        }

        public bool IsValidFileFormat(IFormFile file)
        {
            
            var actualType = MimeTypesMap.GetMimeType(file.FileName);

            if (actualType.StartsWith("text"))
            {
                return true;
            }

            return false;
        }

        public async Task<string> ReadTextFromFile(IFormFile file)
        {
            if (!IsValidFileFormat(file))
            {
                return errorMessageMimeType;
            }

            string html = string.Empty;

            try
            {
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        html = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception)
            {
                return errorMessageGlobal;
            }

            return html;
        }
    }
}
