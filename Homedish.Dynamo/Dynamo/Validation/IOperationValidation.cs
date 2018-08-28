using Homedish.Aws.Dynamo.Models;

namespace Homedish.Aws.Dynamo.Validation
{
    internal interface IOperationValidation<T> where T : OperationModel
    {
        ResponseBase Validate(T query);
    }
}
