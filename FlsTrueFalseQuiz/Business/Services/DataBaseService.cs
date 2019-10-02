using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FlsTrueFalseQuiz.Business.Interfaces;
using log4net;

namespace FlsTrueFalseQuiz.Business.Services
{
    public class DataBaseService : IDataBaseService
    {
        private readonly string _sqlConnectionString;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataBaseService));

        public DataBaseService()
        {
            _sqlConnectionString = ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString;
        }

        public bool TryMapReadLines(string storeProcedureName, SqlParameter[] sqlParameters, Action<SqlDataReader> readerAction)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                using (var command = new SqlCommand(storeProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddRange(sqlParameters);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            readerAction(reader);
                        }
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Logger.Error(nameof(TryMapReadLines), exception);
                return false;
            }
        }

        public bool Execute(string storeProcedureName, SqlParameter[] sqlParameters)
        {
            try
            {
                using (var connection = new SqlConnection(_sqlConnectionString))
                using (var command = new SqlCommand(storeProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddRange(sqlParameters);

                    connection.Open();
                    var affectedCount = command.ExecuteNonQuery();

                    return affectedCount == 1;
                }
            }
            catch (Exception exception)
            {
                Logger.Error(nameof(Execute), exception);
                return false;
            }
        }
    }
}