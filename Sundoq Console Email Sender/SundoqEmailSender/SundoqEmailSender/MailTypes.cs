using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SundoqEmailSender
{
    public enum MailTypes
    {
        Invitation = 0,
        Register = 1,
        Activate = 2,
        Resend_Activate = 3,
        Password_Reset = 4,
        Error = 5,
        HotTopic = 6,
        MorningCoffee = 7
    }
}
