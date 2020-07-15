using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace spatwebapi
{
    public class SendEmail
    {
        public static void SendEmailConfirmation(string subject, string body, string confirmationLink, string emailAddressee)
        {
            //try
            //{
            using (var mailMessage = new MailMessage())
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    //provide credentials
                    client.Credentials = new NetworkCredential("oscarbj03@gmail.com", "zxp-+7.1*");
                    client.EnableSsl = true;

                    // configure the mail message
                    mailMessage.From = new MailAddress("oscarbj03@gmail.com");
                    mailMessage.To.Insert(0, new MailAddress(emailAddressee));
                    mailMessage.Subject = subject;
                    mailMessage.Body = "<a href=" + confirmationLink + ">" + body + "</link>";//"You did it ";
                    mailMessage.IsBodyHtml = true;

                    //send email
                    client.Send(mailMessage);
                }
            }
        }
    }
}
