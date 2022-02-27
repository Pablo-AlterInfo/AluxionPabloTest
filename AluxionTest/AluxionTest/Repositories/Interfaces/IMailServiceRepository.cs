using System.Threading.Tasks;

namespace AluxionTest.Repositories.Interfaces
{
    public interface IMailServiceRepository
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
    }
}
