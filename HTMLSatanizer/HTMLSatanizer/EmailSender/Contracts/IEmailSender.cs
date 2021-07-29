using System.Collections.Generic;
using System.Threading.Tasks;

namespace HTMLSatanizer.EmailSender.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null);
    }
}