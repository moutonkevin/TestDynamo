using System;
using System.Threading.Tasks;
using Homedish.SQL.Models;

namespace Homedish.SQL
{
    public interface IOperations
    {
        Task<T> ExecuteStoredProcedureAsync<T>(StoredProcedureConfiguration configs, Func<SqlReader, Task<T>> readerCallback);
    }
}