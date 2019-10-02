using System;
using System.Configuration;

namespace FlsTrueFalseQuiz.Business.Constants
{
    public class Config
    {
        public static class Settings
        {
            public static int CountOfQuestions => Convert.ToByte(ConfigurationManager.AppSettings["NumberOfQuestionsInQuizSession"] ?? "6");

            public static int QuizSuccessfulThreshold => Convert.ToByte(ConfigurationManager.AppSettings["QuizSuccessfulThreshold"] ?? "4");

            public static string ReplyTo => ConfigurationManager.AppSettings["ReplyTo"] ?? "";
        }
    }
}