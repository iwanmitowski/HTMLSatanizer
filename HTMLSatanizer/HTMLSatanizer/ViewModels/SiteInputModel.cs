using System.ComponentModel.DataAnnotations;

namespace HTMLSatanizer.ViewModels
{
    public class SiteInputModel
    {
        [Required]
        [RegularExpression(@"^(?:(?:https?|ftp)://)?[-a-zA-Z0-9@:\%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:\%_\+.~#?&//=]*)$",
            ErrorMessage = "Invalid URL! Try something else!")]
        public string URL { get; set; }

        public string HTML { get; set; }

        public string SatanizedHTML { get; set; }
    }
}
