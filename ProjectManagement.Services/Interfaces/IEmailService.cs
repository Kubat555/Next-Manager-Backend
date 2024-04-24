using ProjectManagement.Services.Models;

namespace ProjectManagement.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
        string GetHtmlConfirmEmail(string userName, string link);
    }
}
