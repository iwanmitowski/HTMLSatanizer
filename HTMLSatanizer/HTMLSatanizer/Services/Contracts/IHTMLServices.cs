using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HTMLSatanizer.Services.Contracts
{
    public interface IHTMLServices
    {
        string SatanizeHTML(string html);
        Task<string> GetHTMLFromGivenPage(string url);
        public bool IsValidFileFormat(IFormFile file);
        public Task<string> ReadTextFromFile(IFormFile file);
    }
}
