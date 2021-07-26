using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HTMLSatanizer.ViewModels
{
    public class FileInputModel
    {
        [Required]
        public IFormFile File { get; set; }

        public string FileName => "Current file: " + File.FileName;

        public string HTML { get; set; }

        public string SatanizedHTML { get; set; }
    }
}
