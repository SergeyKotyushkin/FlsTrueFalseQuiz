using System;
using System.Net.Mail;
using FlsTrueFalseQuiz.Business.Interfaces;
using log4net;

namespace FlsTrueFalseQuiz.Business.Services
{
    public class MailService : IMailService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MailService));

        public bool Send(MailMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Send(mailMessage);
                    return true;
                }
                catch (Exception exception)
                {
                    Logger.Error(nameof(Send), exception);
                    return false;
                }
            }
        }
    }
}