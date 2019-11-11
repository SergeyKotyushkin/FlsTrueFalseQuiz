using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using FlsTrueFalseQuiz.Business.Constants;
using FlsTrueFalseQuiz.Business.Interfaces;
using FlsTrueFalseQuiz.Business.Models;

namespace FlsTrueFalseQuiz.Business.Services
{
    public class MailGenerator : IMailGenerator
    {
        public MailMessage Generate(Question[] questions, IList<Tuple<string, string>> values, PassGrade grade, string toEmail, int quantity)
        {
            var variationIndex = SelectWordVariationIndex(quantity);
            var otvet = MailGenerator.Otvet[variationIndex];
            var pravilnyj = MailGenerator.Pravilnyj[variationIndex];
            var vernyj = MailGenerator.Vernyj[variationIndex];

            var questionExplanationRows = GetQuestionExplanationRows(questions);

            var extendedValuesList = values.Concat(new[]
            {
                Tuple.Create("%%форма_слова_ответ%%", otvet),
                Tuple.Create("%%форма_слова_правильный%%", pravilnyj),
                Tuple.Create("%%форма_слова_верный%%", vernyj),
                Tuple.Create("%%question_rows%%", questionExplanationRows),
            }).ToList();

            var mailMessage = new MailMessage
            {
                Subject = InjectValues(LoadEmailSubjectTemplate(grade), extendedValuesList),
                Body = InjectValues(LoadEmailTemplate(grade), extendedValuesList),
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(toEmail));
            mailMessage.ReplyToList.Add(Config.Settings.ReplyTo);
            return mailMessage;
        }

        private static string LoadEmailSubjectTemplate(PassGrade grade)
        {
            string templateFileName = null;
            if (grade == PassGrade.Passed)
                templateFileName = "/App_Data/PassedEmailSubjectTemplate.txt";
            else if (grade == PassGrade.Failed)
                templateFileName = "/App_Data/FailedEmailSubjectTemplate.txt";
            else if (grade == PassGrade.Zero)
                templateFileName = "/App_Data/ZeroLevelEmailSubjectTemplate.txt";

            return File.ReadAllText(HostingEnvironment.MapPath(templateFileName));
        }

        private static string LoadEmailTemplate(PassGrade grade)
        {
            string templateFileName = null;
            if (grade == PassGrade.Passed)
                templateFileName = "/App_Data/PassedEmailTemplate.html";
            else if (grade == PassGrade.Failed)
                templateFileName = "/App_Data/FailedEmailTemplate.html";
            else if (grade == PassGrade.Zero)
                templateFileName = "/App_Data/ZeroLevelEmailTemplate.html";

            return File.ReadAllText(HostingEnvironment.MapPath(templateFileName));
        }

        private static string GetQuestionExplanationRows(Question[] questions)
        {
            var questionExplanationRowStringBuilder = new StringBuilder();
            var questionRowTextTextTemplate = LoadQuestionRowTextTextTemplate();
            var questionRowTextImageTemplate = LoadQuestionRowTextImageTemplate();
            for (var i = 0; i < questions.Length; i++)
            {
                var question = questions[i];

                var injectValues = new List<Tuple<string, string>>
                {
                    new Tuple<string, string>("%%text%%", question.Text),
                    new Tuple<string, string>("%%answer%%", question.Answer ? "Правда" : "Вымысел"),
                    new Tuple<string, string>("%%explanation%%", question.Explanation),
                    new Tuple<string, string>("%%column_1_background_color%%", i % 2 == 0 ? "#FFFFFF" : "#F2F2F2"),
                    new Tuple<string, string>("%%column_2_background_color%%", i % 2 == 0 ? "#F2F2F2" : "#FFFFFF")
                };

                var imageRegex = new Regex(@"^http.*(\.jpg|\.png)$", RegexOptions.IgnoreCase);
                var template = imageRegex.IsMatch(question.Explanation)
                    ? questionRowTextImageTemplate
                    : questionRowTextTextTemplate;

                questionExplanationRowStringBuilder.AppendLine(InjectValues(template, injectValues));
            }

            return questionExplanationRowStringBuilder.ToString();
        }

        private static string LoadQuestionRowTextTextTemplate()
        {
            var templateFileName =  "/App_Data/QuestionRowTextTextTemplate.html";

            return File.ReadAllText(HostingEnvironment.MapPath(templateFileName));
        }

        private static string LoadQuestionRowTextImageTemplate()
        {
            var templateFileName =  "/App_Data/QuestionRowTextImageTemplate.html";

            return File.ReadAllText(HostingEnvironment.MapPath(templateFileName));
        }

        private static string InjectValues(string template, IEnumerable<Tuple<string, string>> values)
        {
            var sb = new StringBuilder(template);
            foreach (var value in values)
            {
                sb.Replace(value.Item1, value.Item2);
            }

            return sb.ToString();
        }

        private static readonly string[] Otvet = {"ответ", "ответа", "ответов"};
        private static readonly string[] Vernyj = {"верный", "верных", "верных"};
        private static readonly string[] Pravilnyj = {"правильный", "правильных", "правильных"};

        private static int SelectWordVariationIndex(int quantity)
        {
            var quantityUnder100 = quantity % 100;
            var quantityUnder10 = quantityUnder100 % 10;
            int variationIndex;

            if (quantityUnder100 >= 11 && quantityUnder100 <= 19)
                variationIndex = 2;
            else if (quantityUnder10 == 1)
                variationIndex = 0;
            else if (quantityUnder10 >= 2 && quantityUnder10 <= 4)
                variationIndex = 1;
            else
                variationIndex = 2;

            return variationIndex;
        }
    }
}