using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using FlsTrueFalseQuiz.Business.Interfaces;
using FlsTrueFalseQuiz.Business.Models;

namespace FlsTrueFalseQuiz.Business.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private const string SpGetRandomQuestion = "sp_GetRandomQuestion";

        private readonly IDataBaseService _dataBaseService;

        public QuestionRepository(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public Question GetRandom(IEnumerable<int> excludedQuestionsIds)
        {
            var exludedIdsTable = GetIdsTable(excludedQuestionsIds);

            var sqlParameters = new[] {new SqlParameter("@ExcludedIdsTable", exludedIdsTable)};

            var questionAnswerDtos = new List<QuestionAnswerDto>(1);
            if (!_dataBaseService.TryMapReadLines(
                SpGetRandomQuestion,
                sqlParameters,
                QuestionsReaderAction(questionAnswerDtos)))
            {
                return null;
            }

            var questions = BuildQuestions(questionAnswerDtos);
            return questions.Count == 1 ? questions[0] : null;
        }

        private static DataTable GetIdsTable(IEnumerable<int> excludedQuestionsIds)
        {
            var exludedIdsTable = new DataTable();
            exludedIdsTable.Columns.Add("ID");
            if (excludedQuestionsIds != null)
            {
                foreach (var id in excludedQuestionsIds)
                {
                    exludedIdsTable.Rows.Add(id);
                }
            }

            return exludedIdsTable;
        }

        private static Action<SqlDataReader> QuestionsReaderAction(ICollection<QuestionAnswerDto> questions)
        {
            return reader =>
            {
                questions.Add(new QuestionAnswerDto
                {
                    QuestionId = reader.GetInt32(0),
                    QuestionTitle = reader.GetString(1),
                    QuestionImage = reader.GetString(2),
                    AnswerId = reader.GetInt32(3),
                    AnswerText = reader.GetString(4),
                    AnswerIsValid = reader.GetBoolean(5)
                });
            };
        }

        private static List<Question> BuildQuestions(IEnumerable<QuestionAnswerDto> questionAnswerDtos)
        {
            var questions = questionAnswerDtos
                .GroupBy(q => new {q.QuestionId, q.QuestionTitle, q.QuestionImage})
                .Select(g => new Question
                {
                    Id = g.Key.QuestionId,
                    Text = g.Key.QuestionTitle,
                    ImageUrl = g.Key.QuestionImage,
                    Answers = g.Select(a => new Answer
                    {
                        AnswerId = a.AnswerId,
                        QuestionId = g.Key.QuestionId,
                        Text = a.AnswerText,
                        IsValid = a.AnswerIsValid
                    }).ToList()
                })
                .ToList();

            return questions;
        }
    }
}