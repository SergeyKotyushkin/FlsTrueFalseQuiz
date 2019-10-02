using System.Collections.Generic;
using FlsTrueFalseQuiz.Business.Models;

namespace FlsTrueFalseQuiz.Business.Interfaces
{
    public interface IAnswerRepository
    {
        IEnumerable<Answer> GetByIds(IEnumerable<int> answersIds);
    }
}
