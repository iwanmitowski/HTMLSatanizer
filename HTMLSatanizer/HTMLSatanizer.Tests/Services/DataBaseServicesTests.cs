using HTMLSatanizer.Data;
using HTMLSatanizer.Models;
using HTMLSatanizer.Services;
using HTMLSatanizer.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Tests
{
    class DataBaseServicesTests
    {
        ApplicationDbContext dbContext;
        IDataBaseServices dataBaseServices;
        Site site;
        Site site2;

        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<HTMLSatanizer.Data.ApplicationDbContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;

            dbContext = new ApplicationDbContext(dbContextOptions);

            dataBaseServices = new DataBaseServices(dbContext);

            site = new Site()
            {
                Id = 1,
                URL = "Test.txt",
                HTML = "Hi",
                HTMLSatanized = "Hi",
                CreatedOn = DateTime.UtcNow,
                Type = "File",
            };

            site2 = new Site()
            {
                Id = 2,
                URL = "Test2.txt",
                HTML = "Hi2",
                HTMLSatanized = "Hi2",
                CreatedOn = DateTime.UtcNow,
                Type = "URL",
            };
        }

        [Test]
        public async Task AddShouldAddCorectlyIntoDb()
        {
            await dataBaseServices.Add(site);

            int expected = 1;

            Assert.AreEqual(expected, dataBaseServices.GetAll().Count());
        }

        [TestCase(1)]
        [TestCase(2)]
        public async Task ShouldReturnCorrectObjectById(int id)
        {
            List<Site> sites = new List<Site>() { site, site2 };
            await dataBaseServices.Add(site);
            await dataBaseServices.Add(site2);

            Site expected = sites.FirstOrDefault(x => x.Id == id);

            Assert.AreEqual(expected, dataBaseServices.GetById(id));
        }

        [Test]
        public async Task UpdateShouldUpdateMakeModifiedOnNotNull()
        {
            await dataBaseServices.Add(site);
            await dataBaseServices.Update(site);

            Assert.IsNotNull(dataBaseServices.GetById(site.Id));
        }

        [Test]
        public async Task GetAllShouldReturnExpectedObjects()
        {
            await dataBaseServices.Add(site);
            await dataBaseServices.Add(site2);

            Assert.AreEqual(site, dataBaseServices.GetAll().FirstOrDefault(x => x == site));
            Assert.AreEqual(site2, dataBaseServices.GetAll().FirstOrDefault(x => x == site2));
        }
    }
}
