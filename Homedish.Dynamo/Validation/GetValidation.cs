using Homedish.Dynamo.Models;
using Homedish.Dynamo.Models.Get;

namespace Homedish.Dynamo.Validation
{
    internal class GetValidation : IOperationValidation<GetModel>
    {
        public ResponseBase Validate(GetModel query)
        {
            var response = new GetResponseModel();

            if (string.IsNullOrWhiteSpace(query.TableName))
            {
                response.ErrorMessage = "Table name cannot be empty";
            }

            if (query.PartitionKey == null)
            {
                response.ErrorMessage = "The Primary Partition Key is required";
            }

            return response;
        }
    }
}
