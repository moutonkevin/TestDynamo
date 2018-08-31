using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Homedish.SQL
{
    public class SqlReader
    {
        private SqlDataReader Reader { get; }

        public SqlReader(SqlDataReader reader)
        {
            Reader = reader;
        }

        public async Task<T> GetColumnValue<T>(string columnName)
        {
            var ordinalIndex = Reader.GetOrdinal(columnName);

            return Reader.IsDBNull(ordinalIndex) ? default(T) : await Reader.GetFieldValueAsync<T>(ordinalIndex);
        }

        public bool HasRows()
        {
            return Reader.HasRows;
        }

        public async Task<T> Read<T>(Func<Task<T>> action) where T : class, new()
        {
            if (await Reader.ReadAsync())
            {
                return await action.Invoke();
            }

            return default(T);
        }

        public async Task<IEnumerable<T>> ReadAll<T>(Func<Task<T>> action)
        {
            var results = new List<T>();

            while (await Reader.ReadAsync())
            {
                var iteration = await action.Invoke();

                results.Add(iteration);
            }

            return results;
        }
    }
}
