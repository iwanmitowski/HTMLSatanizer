using HTMLSatanizer.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using MyTested.AspNetCore.Mvc;
using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTMLSatanizer.Tests.Controllers
{
    class ErrorControllerTests
    {
        WebApplicationFactory<Startup> webAppFactory;
        HttpClient client;
        

        [SetUp]
        public void Setup()
        {
            webAppFactory = new WebApplicationFactory<Startup>();
            client = webAppFactory.CreateClient();
        }

        [TestCase(404)]
        [TestCase(500)]
        public void ErrorShouldReturnViewWithErrorViewModel(int id)
             => MyController<ErrorController>
                 .Instance()
                 .Calling(x => x.HttpError(id))
                 .ShouldReturn()
                 .View(view => view
                     .WithModelOfType<int>());

        [TestCase(420)]
        [TestCase(502)]
        [TestCase(102)]
        [TestCase(100)]
        [TestCase(503)]
        public async Task ErrorViewShouldHaveH1TagWithTheGivenStatusCode(int code)
        {
            var response = await client.GetAsync($"Error/HttpError?statusCode={code}");
            var html = await response.Content.ReadAsStringAsync();
            string expected = $"Error ocurred! Status code: {code}</h1>";
            
            Assert.IsTrue(html.Contains(expected));
        }

        [TestCase(404)]
        public async Task StatusCode404ShouldHaveH1TagWithSpecificText(int code)
        {
            var response = await client.GetAsync($"Error/HttpError?statusCode={code}");
            var html = await response.Content.ReadAsStringAsync();
            string expected = $"404 Not Found! Muhahaha</h1>";

            Assert.IsTrue(html.Contains(expected));
        }

        [TestCase(400)]
        public async Task StatusCode400ShouldHaveH1TagWithSpecificText(int code)
        {
            var response = await client.GetAsync($"Error/HttpError?statusCode={code}");
            var html = await response.Content.ReadAsStringAsync();
            string expected = $"You are naughty boy, aren't ya ?!</h1>";

            Assert.IsTrue(html.Contains(expected));
        }

        [TestCase("-404")]
        [TestCase("0")]
        [TestCase("666")]
        [TestCase("1337")]
        [TestCase("1111111111")]
        [TestCase("asd")]
        [TestCase("12312321321312")]
        [TestCase("-404")]
        [TestCase("6664")]
        public async Task InvalidStatusCodeShouldReturn404Page(string code)
        {
            var response = await client.GetAsync($"Error/HttpError?statusCode={code}");
            var html = await response.Content.ReadAsStringAsync();
            string expected = $"404 Not Found! Muhahaha</h1>";

            Assert.IsTrue(html.Contains(expected));
        }
    }
}
