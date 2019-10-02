using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FlsTrueFalseQuiz.Business.Interfaces;
using FlsTrueFalseQuiz.Business.Models;

namespace FlsTrueFalseQuiz.Business.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private const string SpGetAnswersByIds = "sp_GetAnswersByIds";

        private readonly IDataBaseService _dataBaseService;

        public AnswerRepository(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public IEnumerable<Answer> GetByIds(IEnumerable<int> answersIds)
        {
            var answersIdsTable = GetIdsTable(answersIds);

            var sqlParameters = new[] {new SqlParameter("@AnswersIdsTable", answersIdsTable)};

            var answers = new List<Answer>(Constants.Config.Settings.CountOfQuestions);
            if (!_dataBaseService.TryMapReadLines(
                SpGetAnswersByIds,
                sqlParameters,
                AnswersReaderAction(answers)))
            {
                return null;
            }

            return answers;
        }

        private static DataTable GetIdsTable(IEnumerable<int> answersIds)
        {
            var answersIdsTable = new DataTable();
            answersIdsTable.Columns.Add("ID");
            if (answersIds != null)
            {
                foreach (var id in answersIds)
                {
                    answersIdsTable.Rows.Add(id);
                }
            }

            return answersIdsTable;
        }

        private static Action<SqlDataReader> AnswersReaderAction(ICollection<Answer> answers)
        {
            return reader =>
            {
                answers.Add(new Answer
                {
                    AnswerId = reader.GetInt32(0),
                    QuestionId = reader.GetInt32(1),
                    Text = reader.GetString(2),
                    IsValid = reader.GetBoolean(3)
                });
            };
        }
    }
}