using Homedish.Dynamo.Models;
using Homedish.Dynamo.Models.Insert;

namespace Homedish.Dynamo.Validation
{
    internal class InsertValidation : IOperationValidation<InsertModel>
    {
        public ResponseBase Validate(InsertModel query)
        {
            var response = new InsertResponseModel();

            if (string.IsNullOrWhiteSpace(query.TableName))
            {
                response.ErrorMessage = "Table name cannot be empty";
            }

            return response;
        }
    }
}
