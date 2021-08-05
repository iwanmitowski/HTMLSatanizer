using HTMLSatanizer.Services;
using HTMLSatanizer.Services.Contracts;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSatanizer.Tests
{
    public class HTMLServicesTests
    {
        int TextBytes = Encoding.UTF8.GetBytes("This is a dummy file").Length;
        private const string errorMessageHTTPS = "Error occured! Please try something different! Ensure that the site is using HTTPS!";
        IHTMLServices HTMLService;
        HttpClient client;
        List<IFormFile> validFiles;
        List<IFormFile> invalidFiles;

        [SetUp]
        public void Setup()
        {
            client = new HttpClient();
            HTMLService = new HTMLServices(client);

            validFiles = new List<IFormFile>()
            {
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.ics"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.txt"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.html"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.css"),
            };                                                                                   
                                                                                                 
            invalidFiles = new List<IFormFile>()                                                 
            {                                                                                    
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.xsl"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.sh"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.js"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.ehtml"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.mjs"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.xml"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.shtm"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.shtml"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.exe"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.jpg"),
                new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, TextBytes, "Data", "dummy.png"),
            };
        }

        [TestCase("http://gotvach.bg/")]
        [TestCase("https")]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("https://www.vbox7")]
        [TestCase("https://vbox7")]
        [TestCase("https://imrandomlinkandidontevenexist.notevenexist")]
        public async Task GetHTMLFromURLShouldReturnErrorMessageAsHTMLIfLinkIsIncorrect(string url)
        {
            string html = await HTMLService.GetHTMLFromGivenPage(url);
            string expected = errorMessageHTTPS;

            Assert.AreEqual(expected, html);
        }

        [TestCase("<h1></h1>")]
        [TestCase("<a></a>")]
        [TestCase("<li></li>")]
        [TestCase("<ul></ul>")]
        public void SatanizingShouldMakeOpeningHTMLTagsToDevilEmoji(string html)
        {
            string satanized = HTMLService.SatanizeHTML(html);
            string expected = WebUtility.HtmlDecode("&#128520;&#128520;");

            Assert.AreEqual(expected, satanized);
        }

        [TestCase("</h1>")]
        [TestCase("</a>")]
        [TestCase("</li>")]
        [TestCase("</ul>")]
        public void SatanizingClosingTagsShouldReturnEmptyString(string html)
        {
            string satanized = HTMLService.SatanizeHTML(html);
            string expected = string.Empty;

            Assert.AreEqual(expected, satanized);
        }

        [Test]
        public void SatanizingSpecialTagsShouldReturnEmptyString()
        {
            string[] specialTags = "<applet>, <object>, <audio>, <basefont>, <bdo>, <body>, <caption>, <col>, <colgroup>, <dialog>, <frame>, <frameset>, <head>, <hgroup>, <html>, <meta>, <noframes>, <noscript>, <object>, <param>, <picture>, <script>, <source>, <style>, <svg>, <tbody>, <td>, <template>, <tfoot>	, <th>, <thead>, <title>, <track>, <video>".Split(", ");
            string expected = string.Empty;

            foreach (var tag in specialTags)
            {
                string satanized = HTMLService.SatanizeHTML(tag);
                Assert.AreEqual(expected, satanized);
            }
        }

        [Test]
        public void IsValidFileShouldReturnTrueIfFileExtensionIsValid()
        {
            foreach (var file in validFiles)
            {
                Assert.IsTrue(HTMLService.IsValidFileFormat(file));
            }
        }

        [Test]
        public void IsValidFileShouldReturnFalseIfFileExtensionIsNotValid()
        {
            foreach (var file in invalidFiles)
            {
                Assert.IsFalse(HTMLService.IsValidFileFormat(file));
            }
        }

        [Test]
        public async Task ReadTextFromValidFileShouldReturnTheFileContent()
        {
            string expected = "This is a dummy file";

            foreach (var file in validFiles)
            {
                string html = await HTMLService.ReadTextFromFile(file);
                Assert.AreEqual(expected, html);
            }
        }

        [Test]
        public async Task ReadTextFromInValidFileShouldReturnError()
        {
            string expected = "Error Unsupported file extension.";

            foreach (var file in invalidFiles)
            {
                string html = await HTMLService.ReadTextFromFile(file);
                Assert.AreEqual(expected, html);
            }
        }
    }
}