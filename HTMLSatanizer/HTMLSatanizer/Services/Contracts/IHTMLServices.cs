using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services.Contracts
{
    public interface IHTMLServices
    {
        string SatanizeHTML(string html);
    }
}
