using System.Collections.Generic;

namespace Homedish.Dynamo.Models.Delete
{
    public class DeleteModel : OperationModel
    {
        public List<DynamoField> Keys { get; set; }
    }
}
