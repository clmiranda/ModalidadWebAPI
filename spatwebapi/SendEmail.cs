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
            MailMessage mailMessage = new MailMessage("codigo901@gmail.com", emailAddressee);
            mailMessage.Subject = subject;
            mailMessage.Body = "<a href=" + confirmationLink + ">" + body + "</a>";
            mailMessage.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential()
            {
                UserName = "codigo901@gmail.com",
                Password = "zkp97-_+2*"
            };
            client.EnableSsl = true;
            client.Send(mailMessage);
        }
    }
}