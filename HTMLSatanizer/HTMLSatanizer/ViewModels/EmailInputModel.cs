using System.ComponentModel.DataAnnotations;

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
