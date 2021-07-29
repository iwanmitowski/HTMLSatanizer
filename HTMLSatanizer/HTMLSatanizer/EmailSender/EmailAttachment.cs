using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HTMLSatanizer.EmailSender.Contracts
{
    public class EmailAttachment
    {
        public byte[] Content { get; set; }

        public string FileName { get; set; }

        public string MimeType { get; set; }
    }
}
