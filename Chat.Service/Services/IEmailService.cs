using Chat.Service.Models;

namespace Chat.Service.Services
{
    public interface IEmailService
    {
        void sendEmail(MessageEmail message);
    }
}
