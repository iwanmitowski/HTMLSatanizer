using HTMLSatanizer.Data;
using HTMLSatanizer.Models;
using HTMLSatanizer.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services
{
    public class DataBaseServices : IDataBaseServices
    {
        private readonly ApplicationDbContext dbContext;

        public DataBaseServices(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task Add(Site site)
        {
            await dbContext.AddAsync(site);

            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Site site)
        {
            var entry = this.dbContext.Entry(site);

            if (entry.State == EntityState.Detached)
            {
                this.dbContext.Set<Site>().Attach(site);
            }

            site.ModifiedOn = DateTime.UtcNow;
            site.RecentUpdate = (DateTime)site.ModifiedOn;

            this.dbContext.Update(site);
            entry.State = EntityState.Modified;

            await dbContext.SaveChangesAsync();
        }

        public IQueryable<Site> GetAll()
        {
            return this.dbContext.Set<Site>();
        }

        public Site GetById(int Id)
        {
            var site = this.dbContext.Set<Site>().Where(x => x.Id == Id).FirstOrDefault();

            return site;
        }
    }
}
