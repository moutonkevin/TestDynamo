using System.Collections.Generic;
using System.Data.SqlClient;

namespace Homedish.SQL.Extensions
{
    public static class SqlParameterCollectionExtensions
    {
        public static void ToSqlParameters(this SqlParameterCollection sqlParameters, IDictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    sqlParameters.Add(new SqlParameter(parameter.Key, parameter.Value));
                }
            }
        }
    }
}
