using System;

namespace HTMLSatanizer.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
        public string HTML { get; set; }
        public string HTMLSatanized { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
