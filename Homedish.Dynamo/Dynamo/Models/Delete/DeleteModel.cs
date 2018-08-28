using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Models.Delete
{
    public class DeleteModel : OperationModel
    {
        public List<DynamoField> Keys { get; set; }
    }
}
