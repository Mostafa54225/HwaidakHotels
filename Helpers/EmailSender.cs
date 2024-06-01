using System.Net.Mail;
using System.Net;

namespace OrientHGAPI.Helpers
{
    public class EmailSender : IEmailSender
    {
        public void SendMail(string fromAddress, string toAddress, string mailSubject, string mailBody)
        {
            MailMessage mailMsg = new(fromAddress, toAddress, mailSubject, mailBody)
            {
                IsBodyHtml = true,
                Priority = MailPriority.Normal
            };

            SmtpClient smtp = new()
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("do_not_reply@hwaidakhotels.com", "Jkp=zk7:1%O]n?up,wMO"),
                Host = "mail.hwaidakhotels.com",
                Port = 587
            };

            smtp.Send(mailMsg);
        }
    }
}
