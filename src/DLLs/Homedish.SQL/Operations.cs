using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Homedish.SQL.Extensions;
using Homedish.SQL.Models;

namespace Homedish.SQL
{
    public class Operations : IOperations
    {
        public async Task<T> ExecuteStoredProcedure<T>(StoredProcedureConfiguration configs, Func<SqlReader, Task<T>> readerCallback)
        {
            using (var connection = new SqlConnection(configs.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = configs.StoredProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = configs.ExecutionTimeout;

                command.Parameters.ToSqlParameters(configs.StoredProcedureParameters);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    return await readerCallback.Invoke(new SqlReader(reader));
                }
            }
        }
    }
}
