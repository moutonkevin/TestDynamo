using System.Collections.Generic;

namespace Homedish.Dynamo.Models.Query
{
    public class QueryResponseModel : ResponseBase
    {
        public List<List<DynamoField>> Items { get; set; }
    }
}
