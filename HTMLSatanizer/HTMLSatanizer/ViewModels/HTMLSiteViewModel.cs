using System;

namespace HTMLSatanizer.ViewModels
{
    public class HTMLSiteViewModel
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Type { get; set; }
        public string HTML { get; set; }
        public string ShortHTML => HTML.Length > 300 ? HTML.Substring(0, 300) + "..." : HTML;
        public string HTMLSatanized { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime RecentUpdate => (DateTime)(CreatedOn < ModifiedOn ? ModifiedOn : CreatedOn);

    }
}
