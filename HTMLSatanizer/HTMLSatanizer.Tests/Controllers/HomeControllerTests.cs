using HTMLSatanizer.Controllers;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.Testing;
using MyTested.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTMLSatanizer.ViewModels;
using System.Security.Cryptography.X509Certificates;
using HTMLSatanizer.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using HTMLSatanizer.Data;
using HTMLSatanizer.Services;
using Moq;
using HTMLSatanizer.Models;

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
                  instance.WithDependencies(c=>c.With(dataBaseServices)))
                .Calling(c => c.Index())                        
                .ShouldReturn()                                 
                .View(result => result                            
                    .WithModelOfType<List<HTMLSiteViewModel>>());

    }
}
