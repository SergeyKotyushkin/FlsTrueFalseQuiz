using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using FlsTrueFalseQuiz.Business.Constants;
using FlsTrueFalseQuiz.Business.Interfaces;
using FlsTrueFalseQuiz.Business.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlsTrueFalseQuiz.Controllers.Result
{
    public class ResultController : Controller
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ResultController));

        private static JsonSerializerSettings JsonSerializerSettings =>
            new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

        private readonly IResultRepository _resultRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMailService _mailService;
        private readonly IMailGenerator _mailGenerator;

        public ResultController(
            IResultRepository resultRepository,
            IQuestionRepository questionRepository,
            IMailService mailService,
            IMailGenerator mailGenerator)
        {
            _resultRepository = resultRepository;
            _questionRepository = questionRepository;
            _mailService = mailService;
            _mailGenerator = mailGenerator;
        }

        [HttpPost]
        public string SaveResults(string email, string name, string stack, string phone, string comment, UserAnswer[] userAnswers)
        {
            email = email.Trim().ToLower(CultureInfo.GetCultureInfo("ru-RU"));
            if (userAnswers == null || userAnswers.Length == 0)
            {
                return JsonConvert.SerializeObject(new {HasErrors = true, MailSent = false}, JsonSerializerSettings);
            }

            string errorJson;
            if (!ValidateEmail(email, out errorJson) &&
                !(email.Equals("Darya.Kvasova@firstlinesoftware.com", StringComparison.CurrentCultureIgnoreCase)))
            {
                return errorJson;
            }

            var questions = _questionRepository.GetQuestions(userAnswers.Select(x => x.QuestionId));
            if (questions == null)
            {
                return JsonConvert.SerializeObject(new {HasErrors = true, MailSent = false},
                    JsonSerializerSettings);
            }

            var answers = questions
                .Join(
                    userAnswers,
                    q => q.Id,
                    a => a.QuestionId,
                    (q, a) => new
                    {
                        q.Id,
                        q.Text,
                        q.Answer,
                        q.Explanation,
                        UserAnswer = a.Answer,
                        IsCorrect = q.Answer == a.Answer
                    })
                .ToArray();

            var answersToSave = answers.Select(x => new { QuestionId = x.Id, x.Answer, x.UserAnswer, x.IsCorrect });
            var correctAnswers = answers.Where(a => a.IsCorrect).ToArray();
            var countOfCorrectAnswers = correctAnswers.Length;
            var totalNumberOfQuestions = Config.Settings.CountOfQuestions;
            var quizPassedThreshold = Config.Settings.QuizSuccessfulThreshold;
            var passed = countOfCorrectAnswers >= quizPassedThreshold;
            var passGrade = passed ? PassGrade.Passed : (countOfCorrectAnswers > 0 ? PassGrade.Failed : PassGrade.Zero);

            if (!TrySendMail(email, name, passGrade, questions, countOfCorrectAnswers, totalNumberOfQuestions, out errorJson))
            {
                string trySaveResult;
                TrySaveResult(email, name, stack, phone, comment, false, answersToSave, countOfCorrectAnswers, out trySaveResult);

                return errorJson;
            }

            if (!TrySaveResult(email, name, stack, phone, comment, true, answersToSave, countOfCorrectAnswers, out errorJson))
            {
                return errorJson;
            }

            return JsonConvert.SerializeObject(new { }, JsonSerializerSettings);
        }

        private bool ValidateEmail(string email, out string errorJson)
        {
            errorJson = string.Empty;

            var emailCheck = _resultRepository.TestEmail(email);
            if (!emailCheck.HasValue)
            {
                errorJson =
                    JsonConvert.SerializeObject(new {HasErrors = true, MailSent = false}, JsonSerializerSettings);
                return false;
            }

            if (!emailCheck.Value)
            {
                errorJson = JsonConvert.SerializeObject(new {HasErrors = true, MailSent = false, UsedEmail = true},
                    JsonSerializerSettings);
                return false;
            }

            return true;
        }

        private bool TrySendMail(string email, string name, PassGrade passGrade, IEnumerable<Business.Models.Question> questions, int countOfCorrectAnswers, int totalQuestions, out string errorJson)
        {
            errorJson = string.Empty;

            var values = new List<Tuple<string, string>>
                { Tuple.Create("%%email%%", email),
                  Tuple.Create("%%name%%", name),
                  Tuple.Create("%%correct_anwsers%%", countOfCorrectAnswers.ToString()),
                  Tuple.Create("%%total_questions%%", totalQuestions.ToString())
                };

            var isEmailSent = false;
            var errorWhenSendingEmail = false;
            try
            {
                using (var message = _mailGenerator.Generate(questions, values, passGrade, email, countOfCorrectAnswers))
                {
                    isEmailSent = _mailService.Send(message);
                    errorWhenSendingEmail = !isEmailSent;
                }
            }
            catch (Exception e)
            {
                Logger.Error(nameof(TrySendMail), e);
            }

            if (!isEmailSent)
            {
                errorJson = JsonConvert.SerializeObject(new {HasErrors = true, MailSent = false, MailSendError = errorWhenSendingEmail },
                    JsonSerializerSettings);
                return false;
            }

            return true;
        }

        private bool TrySaveResult(
            string email, 
            string name, 
            string stack, 
            string phone, 
            string comment,
            bool emailSent,
            IEnumerable answersArray,
            int countOfCorrectAnswers, 
            out string errorJson)
        {
            errorJson = string.Empty;

            var saveResult = _resultRepository.SaveResult(
                email,
                JsonConvert.SerializeObject(new {answersArray}, JsonSerializerSettings),
                countOfCorrectAnswers,
                Config.Settings.CountOfQuestions,
                emailSent,
                name ?? string.Empty,
                stack ?? string.Empty,
                phone ?? string.Empty,
                comment ?? string.Empty);

            if (!saveResult)
            {
                errorJson = JsonConvert.SerializeObject(new {HasErrors = true, MailSent = emailSent},
                    JsonSerializerSettings);
                return false;
            }

            return true;
        }
    }
}