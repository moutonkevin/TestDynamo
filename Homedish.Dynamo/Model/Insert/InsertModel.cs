using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Model.Insert
{
    public class InsertModel : OperationModel
    {
        public List<DynamoField> Fields { get; set; }
    }
}