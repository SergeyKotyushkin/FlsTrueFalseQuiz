namespace FlsTrueFalseQuiz.Business.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }

        public int QuestionId { get; set; }

        public string Text { get; set; }

        public bool IsValid { get; set; }
    }
}