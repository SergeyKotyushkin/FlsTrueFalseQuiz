using System.Collections.Generic;
using FlsTrueFalseQuiz.Business.Models;

namespace FlsTrueFalseQuiz.Business.Interfaces
{
    public interface IQuestionRepository
    {
        Question GetRandom(IEnumerable<int> excludedQuestionsIds);

        Question[] GetQuestions(IEnumerable<int> ids);
    }
}
