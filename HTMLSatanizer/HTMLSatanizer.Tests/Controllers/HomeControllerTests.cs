using HTMLSatanizer.Controllers;
using HTMLSatanizer.Services.Contracts;
using HTMLSatanizer.ViewModels;
using Moq;
using MyTested.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;

namespace HTMLSatanizer.Tests.Controllers
{
    class HomeControllerTests
    {
        IDataBaseServices dataBaseServices;

        [SetUp]
        public void Setup()
        {
            var mockServices = new Mock<IDataBaseServices>();
            dataBaseServices = mockServices.Object;
        }

        [Test]
        public void ReturnViewWithCorrectModelWhenCallingIndexAction()
            => MyController<HomeController>
                  .Instance(instance =>
                  instance.WithDependencies(c => c.With(dataBaseServices)))
                .Calling(c => c.Index())
                .ShouldReturn()
                .View(result => result
                    .WithModelOfType<List<HTMLSiteViewModel>>());
    }
}
