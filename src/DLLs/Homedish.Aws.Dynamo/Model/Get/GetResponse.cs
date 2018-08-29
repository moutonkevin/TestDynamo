using System.Collections.Generic;

namespace Homedish.Aws.Dynamo.Model.Get
{
    public class GetResponseModel : ResponseBase
    {
        public List<List<DynamoField>> Items { get; set; }
    }
}