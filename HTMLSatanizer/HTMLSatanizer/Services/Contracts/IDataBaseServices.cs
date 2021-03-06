using HTMLSatanizer.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services.Contracts
{
    public interface IDataBaseServices
    {
        Task Update(Site site);
        Task Add(Site site);
        IQueryable<Site> GetAll();
        Site GetById(int Id);
    }
}
