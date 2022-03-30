using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SpringBoard.Service
{
    public class ServiceMail:IServiceMail
    {
        private readonly IConfiguration _configuration;

        public ServiceMail(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string sendMail(string mail, string obj, string body)
        {
            try
            {
                

                string sendermail = _configuration["MailSettings:Mail"].ToString();
                string senderpassword = _configuration["MailSettings:Password"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 1000000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                string from = sendermail;

                MailMessage mailMessage = new MailMessage(from, mail);



                mailMessage.Subject = obj;
                mailMessage.Body = body;

                client.Credentials = new NetworkCredential(sendermail, senderpassword);

                mailMessage.IsBodyHtml = true;

                mailMessage.BodyEncoding = UTF8Encoding.UTF8;

                client.Send(mailMessage);

            }
            catch (Exception e)
            {

                return "error occured";

            }
            return "success";

        }
    }
}
