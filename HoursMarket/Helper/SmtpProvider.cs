using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HoursMarket.Helper
{
    public class SmtpProvider : IEmailSender
    {

      
        public void SendEmail(string recepients, string title, string body)
        {


#if DEBUG

            var fromAddress = new MailAddress(Startup.StaticConfig["Secrets:Email"], "Hours Market No Reply");
            string fromPassword = Startup.StaticConfig["Secrets:EmailPassword"];
#elif RELEASE
            var fromAddress = new MailAddress(Startup.StaticConfig["Credentials:Email"], "Hours Market No Reply");
            string fromPassword = Startup.StaticConfig["Credentials:EmailPassword"];
#endif


            using (var smtp = new SmtpClient
            {
                Host = "smtp.webio.pl",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            })
            {
                using (var message = new MailMessage()
                {
                    Subject = title,
                    Body = body,
                })
                {
                    message.From = fromAddress;
                    foreach (var address in recepients.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.To.Add(address);
                    }

                 
                        smtp.Send(message);
                    
                 
                }
            }




        }
    }
}
