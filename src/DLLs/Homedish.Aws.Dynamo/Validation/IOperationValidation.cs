using Homedish.Aws.Dynamo.Model;

namespace Homedish.Aws.Dynamo.Validation
{
    internal interface IOperationValidation<in T> where T : OperationModel
    {
        ResponseBase Validate(T query);
    }
}
