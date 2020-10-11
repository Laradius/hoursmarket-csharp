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

        private readonly IConfiguration _config;
        public SmtpProvider(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void SendEmail(string recepients, string title, string body)
        {


            var fromAddress = new MailAddress(_config["Secrets:Email"], "Hours Market No Reply");
            string fromPassword = _config["Secrets:EmailPassword"];



            using (var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
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
