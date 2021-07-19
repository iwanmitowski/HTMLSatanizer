using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services.Contracts
{
    public interface IMimeTypeServices
    {
        public string GetContentType(string fileName);
    }
}