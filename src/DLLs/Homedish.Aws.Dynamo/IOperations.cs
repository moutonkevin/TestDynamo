using System.Threading.Tasks;
using Homedish.Aws.Dynamo.Model.Delete;
using Homedish.Aws.Dynamo.Model.Get;
using Homedish.Aws.Dynamo.Model.Insert;
using Homedish.Aws.Dynamo.Model.Query;

namespace Homedish.Aws.Dynamo
{
    public interface IOperations
    {
        Task<InsertResponseModel> InsertAsync(InsertModel value);
        Task<GetResponseModel> GetAsync(GetModel value);
        Task<QueryResponseModel> QueryAsync(QueryModel value);
        Task<DeleteResponseModel> DeleteAsync(DeleteModel value);
    }
}
