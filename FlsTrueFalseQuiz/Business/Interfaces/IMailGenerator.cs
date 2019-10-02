using System;
using System.Collections.Generic;
using System.Net.Mail;
using FlsTrueFalseQuiz.Business.Constants;

namespace FlsTrueFalseQuiz.Business.Interfaces
{
    public interface IMailGenerator
    {
        MailMessage Generate(IList<Tuple<string, string>> values, PassGrade grade, string toEmail, int quantity);
    }
}