using HTMLSatanizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services.Contracts
{
    public interface IDataBaseServices
    {
        void Update(Site site);
        Task Add(Site site);
    }
}
