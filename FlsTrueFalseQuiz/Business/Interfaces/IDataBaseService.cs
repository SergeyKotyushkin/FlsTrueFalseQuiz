using System;
using System.Data.SqlClient;

namespace FlsTrueFalseQuiz.Business.Interfaces
{
    public interface IDataBaseService
    {
        bool TryMapReadLines(string storeProcedureName, SqlParameter[] sqlParameters, Action<SqlDataReader> readerAction);

        bool Execute(string storeProcedureName, SqlParameter[] sqlParameters);
    }
}