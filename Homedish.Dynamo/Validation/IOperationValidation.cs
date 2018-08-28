using Homedish.Dynamo.Models;

namespace Homedish.Dynamo.Validation
{
    internal interface IOperationValidation<T> where T : OperationModel
    {
        ResponseBase Validate(T query);
    }
}
