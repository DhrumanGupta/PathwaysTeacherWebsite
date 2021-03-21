using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Website.Services
{
    public interface IEmailService
    {
        void Send(string sendTo, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string to, string subject, string body)
        {
            MailAddress fromAddress = new MailAddress(_configuration["SenderEmail"], "Pathways School Noida");
            MailAddress toAddress = new MailAddress(to);

            using (var message = new MailMessage(fromAddress, toAddress))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = GetNewClient())
                {
                    try
                    {
                        client.Send(message);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex);
                    }
                }
            }
        }

        private SmtpClient GetNewClient()
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            var cre = new NetworkCredential(_configuration["SenderEmail"], _configuration["Password"]);
            client.Credentials = cre;

            return client;
        }
    }
}
