using Homedish.Aws.Dynamo.Model;

namespace Homedish.Aws.Dynamo.Validation
{
    internal interface IOperationValidation<T> where T : OperationModel
    {
        ResponseBase Validate(T query);
    }
}
