using System.Collections.Generic;

namespace Homedish.Dynamo.Models.Query
{
    public class QueryModel : OperationModel
    {
        public string IndexName { get; set; }
        public string ConditionExpression { get; set; }
        public List<DynamoField> ConditionExpressionValues { get; set; }
        public List<DynamoFieldSchema> FieldsToRetrieve { get; set; }
    }
}
