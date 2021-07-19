using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
