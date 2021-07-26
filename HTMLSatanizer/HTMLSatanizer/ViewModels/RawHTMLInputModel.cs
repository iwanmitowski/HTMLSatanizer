using System.ComponentModel.DataAnnotations;

namespace HTMLSatanizer.ViewModels
{
    public class RawHTMLInputModel
    {
        [Required]
        public string HTML { get; set; }

        public string SatanizedHTML { get; set; }

    }
}
