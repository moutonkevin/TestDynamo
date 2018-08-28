using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Models.Insert
{
    public class InsertModel : OperationModel
    {
        public List<DynamoField> Fields { get; set; }
    }
}
