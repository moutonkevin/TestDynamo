using Homedish.Aws.Dynamo.Model;
using Homedish.Aws.Dynamo.Model.Query;

namespace Homedish.Aws.Dynamo.Validation
{
    internal class QueryValidation : IOperationValidation<QueryModel>
    {
        public ResponseBase Validate(QueryModel query)
        {
            var response = new QueryResponseModel();

            if (string.IsNullOrWhiteSpace(query.TableName))
            {
                response.ErrorMessage = "Table name cannot be empty";
            }

            if (string.IsNullOrWhiteSpace(query.IndexName))
            {
                response.ErrorMessage = "Index name cannot be empty";
            }

            return response;
        }
    }
}
