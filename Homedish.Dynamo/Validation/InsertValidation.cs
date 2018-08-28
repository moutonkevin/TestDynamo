using Homedish.Aws.Dynamo.Model;
using Homedish.Aws.Dynamo.Model.Insert;

namespace Homedish.Aws.Dynamo.Validation
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
