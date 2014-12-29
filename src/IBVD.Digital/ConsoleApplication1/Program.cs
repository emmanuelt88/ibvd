using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            using (MailMessage mm = new MailMessage("emmanuelt88@gmail.com", "emmanuelt88@gmail.com"))
            {
                mm.Subject = "IBVD - Nueva consulta ";
                mm.Body = "hola";
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 20000;
                NetworkCredential NetworkCred = new NetworkCredential("emmanuelt88@gmail.com", "Tecnoquad_14");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}
