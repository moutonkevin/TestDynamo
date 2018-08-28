using Homedish.Dynamo.Models;
using Homedish.Dynamo.Models.Delete;

namespace Homedish.Dynamo.Validation
{
    internal class DeleteValidation : IOperationValidation<DeleteModel>
    {
        public ResponseBase Validate(DeleteModel query)
        {
            var response = new DeleteResponseModel();

            if (string.IsNullOrWhiteSpace(query.TableName))
            {
                response.ErrorMessage = "Table name cannot be empty";
            }

            if (query.Keys == null || query.Keys.Count == 0)
            {
                response.ErrorMessage = "The Primary Partition Key is required";
            }

            return response;
        }
    }
}
