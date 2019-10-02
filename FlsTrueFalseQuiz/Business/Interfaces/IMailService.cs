using System.Net.Mail;

namespace FlsTrueFalseQuiz.Business.Interfaces
{
    public interface IMailService
    {
        bool Send(MailMessage mailMessage);
    }
}