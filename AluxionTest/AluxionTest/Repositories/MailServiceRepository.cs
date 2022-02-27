using AluxionTest.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace AluxionTest.Repositories
{
    public class MailServiceRepository : IMailServiceRepository
    {
        private IConfiguration _configuration;

        public MailServiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = _configuration["AppSettings:SendGridApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Pabloizquierdo2395@gmail.com", "Pablo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
