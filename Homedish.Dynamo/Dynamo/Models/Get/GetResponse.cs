using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Models.Get
{
    public class GetResponseModel : ResponseBase
    {
        public List<List<DynamoField>> Items { get; set; }
    }
}
