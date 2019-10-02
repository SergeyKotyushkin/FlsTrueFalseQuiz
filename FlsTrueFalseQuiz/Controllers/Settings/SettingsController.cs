using System.Web.Mvc;
using FlsTrueFalseQuiz.Business.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FlsTrueFalseQuiz.Controllers.Settings
{
    public class SettingsController : Controller
    {
        private static JsonSerializerSettings JsonSerializerSettings =>
            new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};

        public string QuizOptions()
        {
            var settings = new
            {
                Config.Settings.CountOfQuestions
            };

            return JsonConvert.SerializeObject(new {settings}, JsonSerializerSettings);
        }
    }
}