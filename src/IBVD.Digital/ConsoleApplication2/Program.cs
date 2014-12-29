using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Rextester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //MailMessage mail = new MailMessage();
                //mail.From = new MailAddress("mail@iglesiadominico.com.ar", "Enquiry");
                //mail.To.Add("emmanuelt88@gmail.com");
                //mail.IsBodyHtml = true;
                //mail.Subject = "Registration";
                //mail.Body = "Some Text";
                //mail.Priority = MailPriority.High;

                //SmtpClient smtp = new SmtpClient("mail.iglesiadominico.com.ar", 25);
                //SmtpClient smtp = new SmtpClient();
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = new System.Net.NetworkCredential("mail@iglesiadominico.com.ar", "asdasd");
                //smtp.EnableSsl = false;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                //smtp.Send(mail);

                using (MailMessage mm = new MailMessage())
                {
                    mm.From = new MailAddress("mail@iglesiadominico.com.ar");
                    mm.To.Add("emmanuelt88@gmail.com");
                    mm.Subject = "subject";
                    mm.Body = "hplaasdsd";
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();

                    NetworkCredential NetworkCred = new NetworkCredential();
                    smtp.Send(mm);
                }

                Console.WriteLine("Todo piola");
            }
            catch (Exception e)
            {
                //Your code goes here
                Console.WriteLine(e.Message);
            }

            Console.ReadKey();

           
        }
    }
}