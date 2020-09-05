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
        public void SendEmail(string recepient, string title, string body)
        {


            var fromAddress = new MailAddress(_config["Secrets:Email"], "Hours Market No Reply");
            var toAddress = new MailAddress(recepient);
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
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = title,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }




        }
    }
}
