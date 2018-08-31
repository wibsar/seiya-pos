using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Seiya
{
    public class Notification
    {
        /// <summary>
        /// Smpt email notification 
        /// </summary>
        /// <param name="toName"></param>
        /// <param name="toEmailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="fromName"></param>
        /// <returns></returns>
        public static bool SendNotification(string toName, string toEmailAddress, string subject, string body, string fromName = "Wibsar Bot")
        {
            if (string.IsNullOrEmpty(toEmailAddress) || string.IsNullOrEmpty(toName) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body))
            {
                throw new ArgumentException();
            }

            try
            {
                var fromAddress = new MailAddress("notification@wibsar.com", fromName);
                var toAddress = new MailAddress(toEmailAddress, toName);
                const string fromPassword = "pass123";

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
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                var exception = e;
                var errorMessage = e.Message;
                return false;
            }
        }
    }
}
