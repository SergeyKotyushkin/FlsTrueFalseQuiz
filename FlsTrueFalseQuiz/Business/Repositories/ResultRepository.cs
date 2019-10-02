using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using FlsTrueFalseQuiz.Business.Interfaces;

namespace FlsTrueFalseQuiz.Business.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private const string SpTestResultEmail = "sp_TestResultEmail";
        private const string SpSaveTestResult = "sp_SaveTestResult";

        private readonly IDataBaseService _dataBaseService;

        public ResultRepository(IDataBaseService dataBaseService)
        {
            _dataBaseService = dataBaseService;
        }

        public bool? TestEmail(string email)
        {
            var sqlParameters = new[] {new SqlParameter("@Email", email)};

            var counts = new List<int>(1);
            if (!_dataBaseService.TryMapReadLines(
                SpTestResultEmail,
                sqlParameters,
                CountReaderAction(counts)))
            {
                return null;
            }

            return counts.Count == 1 && counts[0] == 0;
        }

        public bool SaveResult(
            string email, 
            string answers, 
            int validCount, 
            int totalCount, 
            bool emailSent, 
            string name, 
            string stack,
            string phone,
            string comment)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Answers", answers),
                new SqlParameter("@ValidCount", validCount),
                new SqlParameter("@TotalCount", totalCount),
                new SqlParameter("@EmailSent", emailSent),
                new SqlParameter("@Nick", name),
                new SqlParameter("@Stack", stack),
                new SqlParameter("@Phone", phone),
                new SqlParameter("@Comment", comment)
            };

            return _dataBaseService.Execute(SpSaveTestResult, sqlParameters);
        }

        private static Action<SqlDataReader> CountReaderAction(ICollection<int> counts)
        {
            return reader =>
            {
                counts.Add(reader.GetInt32(0));
            };
        }
    }
}