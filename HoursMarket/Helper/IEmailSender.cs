using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoursMarket.Helper
{
    public interface IEmailSender
    {

        public void SendEmail(string recepient, string title, string body);

    }
}
