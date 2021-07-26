using HTMLSatanizer.Models;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services.Contracts
{
    public interface IDataBaseServices
    {
        void Update(Site site);
        Task Add(Site site);
    }
}
