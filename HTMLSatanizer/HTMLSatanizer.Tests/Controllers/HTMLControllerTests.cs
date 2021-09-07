using HTMLSatanizer.Controllers;
using HTMLSatanizer.Data;
using HTMLSatanizer.EmailSender.Contracts;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Moq;
using MyTested.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLSatanizer.Tests.Controllers
{
    class HTMLControllerTests
    {
        IHTMLServices htmlServices;
        ApplicationDbContext dbContext;
        IDataBaseServices dataBaseServices;
        IEmailSender emailSender;
        SiteInputModel emptySiteInputModel;

        [SetUp]
        public void Setup()
        {
            var mockHTMLServices = new Mock<IHTMLServices>();
            htmlServices = mockHTMLServices.Object;
            var mockDbContext = new Mock<ApplicationDbContext>();
            dbContext = mockDbContext.Object;
            var mockDatabaseServices = new Mock<IDataBaseServices>();
            dataBaseServices = mockDatabaseServices.Object;
            var mockEmailSender = new Mock<IEmailSender>();
            emailSender = mockEmailSender.Object;
        }

        [Test]
        public void URLShouldReturnView()
            => MyController<HTMLController>
                  .Instance(instance =>
                  instance
                  .WithDependencies(c => c
                    .With(htmlServices)
                    .With(dbContext)
                    .With(dataBaseServices)
                    .With(emailSender)))
                .Calling(c => c.URL())
                .ShouldReturn()
                .View();

        [Test]
        public void URLShouldRedirectTo400ErrorPageIfHTMLIsNull()
            => MyController<HTMLController>
                  .Instance(instance =>
                  instance
                  .WithDependencies(c => c
                    .With(htmlServices)
                    .With(dbContext)
                    .With(dataBaseServices)
                    .With(emailSender)))
                .Calling(c => c.URL(new SiteInputModel()))
                .ShouldReturn()
                .Redirect("/Error/HttpError?statusCode=400");

        //[TestCase("<h1>", "https://www.google.com/")]
        //[TestCase("test", "https://www.google.com/")]
        //[TestCase("Error", "https://www.google.com/")]
        //[TestCase(@"""", "https://www.google.com/")]
        //public void URLShouldReturnViewIfHTMLNotNull(string html, string url)
        //    => MyController<HTMLController>
        //          .Instance(instance =>
        //          instance
        //          .WithDependencies(c => c
        //            .With(htmlServices)
        //            .With(dbContext)
        //            .With(dataBaseServices)
        //            .With(emailSender)))
        //        .Calling(c => c.URL(new SiteInputModel() { HTML = html, URL = url }))
        //        .ShouldHave()
        //        .ValidModelState()
        //        .AndAlso()
        //        .ShouldReturn()
        //        .View();

        [Test]
        public void RawHTMLShouldReturnView()
           => MyController<HTMLController>
                 .Instance(instance =>
                 instance
                 .WithDependencies(c => c
                   .With(htmlServices)
                   .With(dbContext)
                   .With(dataBaseServices)
                   .With(emailSender)))
               .Calling(c => c.RawHTML())
               .ShouldReturn()
               .View();

        [Test]
        public void RawHTMLShouldRedirectTo400ErrorPageIfHTMLIsNull()
            => MyController<HTMLController>
                  .Instance(instance =>
                  instance
                  .WithDependencies(c => c
                    .With(htmlServices)
                    .With(dbContext)
                    .With(dataBaseServices)
                    .With(emailSender)))
                .Calling(c => c.RawHTML(new RawHTMLInputModel()))
                .ShouldReturn()
                .Redirect("/Error/HttpError?statusCode=400");

        //[TestCase("<h1>")]
        //[TestCase("test")]
        //[TestCase("Error")]
        //[TestCase(@"""")]
        //public void RawHTMLShouldReturnViewIfHTMLNotNull(string html)
        //    => MyController<HTMLController>
        //          .Instance(instance =>
        //          instance
        //          .WithDependencies(c => c
        //            .With(htmlServices)
        //            .With(dbContext)
        //            .With(dataBaseServices)
        //            .With(emailSender)))
        //        .Calling(c => c.RawHTML(new RawHTMLInputModel() { HTML = html }))
        //        .ShouldReturn()
        //        .Redirect("/Error/HttpError?statusCode=400");
    }
}
