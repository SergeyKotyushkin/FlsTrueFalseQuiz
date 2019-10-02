namespace FlsTrueFalseQuiz.Business.Models
{
    public class QuestionAnswerDto
    {
        public int QuestionId { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionImage { get; set; }

        public int AnswerId { get; set; }

        public string AnswerText { get; set; }

        public bool AnswerIsValid { get; set; }
    }
}