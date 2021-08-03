using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.ViewModels
{
    public class EmailInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int Id { get; set; }
    }
}
