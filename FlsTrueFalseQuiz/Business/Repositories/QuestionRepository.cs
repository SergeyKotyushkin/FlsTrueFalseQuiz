using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FlsTrueFalseQuiz.Business.Interfaces;
using FlsTrueFalseQuiz.Business.Models;

namespace FlsTrueFalseQuiz.Business.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private const string SpGetRandomQuestion = "sp_GetRandomQuestion";
        private const string SpGetQuestions = "sp_GetQuestions";

        private readonly IDataBaseService _dataBaseService;

        public QuestionRepository(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public Question GetRandom(IEnumerable<int> excludedQuestionsIds)
        {
            var exludedIdsTable = GetIdsTable(excludedQuestionsIds);

            var sqlParameters = new[] {new SqlParameter("@ExcludedIdsTable", exludedIdsTable)};

            var questions = new List<Question>(1);
            if (!_dataBaseService.TryMapReadLines(
                SpGetRandomQuestion,
                sqlParameters,
                RandomQuestionsReaderAction(questions)))
            {
                return null;
            }

            return questions.Count == 1 ? questions[0] : null;
        }

        public Question[] GetQuestions(IEnumerable<int> ids)
        {
            var idsTable = GetIdsTable(ids);

            var sqlParameters = new[] {new SqlParameter("@IdsTable", idsTable)};

            var questions = new List<Question>(1);
            if (!_dataBaseService.TryMapReadLines(
                SpGetQuestions,
                sqlParameters,
                FullQuestionsReaderAction(questions)))
            {
                return null;
            }

            return questions.ToArray();
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

        private static Action<SqlDataReader> RandomQuestionsReaderAction(ICollection<Question> questions)
        {
            return reader =>
            {
                questions.Add(new Question
                {
                    Id = reader.GetInt32(0),
                    Text = reader.GetString(1)
                });
            };
        }

        private static Action<SqlDataReader> FullQuestionsReaderAction(ICollection<Question> questions)
        {
            return reader =>
            {
                questions.Add(new Question
                {
                    Id = reader.GetInt32(0),
                    Text = reader.GetString(1),
                    Answer = reader.GetBoolean(2),
                    Explanation = reader.GetString(3)
                });
            };
        }
    }
}