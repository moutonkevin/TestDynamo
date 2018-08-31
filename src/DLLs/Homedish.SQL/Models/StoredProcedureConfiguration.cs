using System.Collections.Generic;

namespace Homedish.SQL.Models
{
    public class StoredProcedureConfiguration
    {
        public string ConnectionString { get; set; }
        public string StoredProcedureName { get; set; }
        public IDictionary<string, object> StoredProcedureParameters { get; set; }
        public int ExecutionTimeout { get; set; } = 30;
    }
}
