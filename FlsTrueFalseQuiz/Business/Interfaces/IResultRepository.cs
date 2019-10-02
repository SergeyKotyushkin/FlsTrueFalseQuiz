namespace FlsTrueFalseQuiz.Business.Interfaces
{
    public interface IResultRepository
    {
        bool? TestEmail(string email);

        bool SaveResult(
            string email,
            string answers,
            int validCount,
            int totalCount,
            bool emailSent,
            string name,
            string stack,
            string phone,
            string comment);
    }
}