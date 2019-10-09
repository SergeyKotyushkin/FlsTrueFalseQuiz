namespace FlsTrueFalseQuiz.Business.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool Answer { get; set; }

        public string Explanation { get; set; }
    }
}