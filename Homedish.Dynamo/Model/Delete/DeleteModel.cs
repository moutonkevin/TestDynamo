using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Model.Delete
{
    public class DeleteModel : OperationModel
    {
        public List<DynamoField> Keys { get; set; }
    }
}