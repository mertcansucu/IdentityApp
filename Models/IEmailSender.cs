using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityApp.Models
{
    //interface file
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);//subject emailin konu bilgisi,message içeriği
    }
}