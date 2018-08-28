using System.Threading.Tasks;
using Homedish.Aws.Dynamo.Models.Delete;
using Homedish.Aws.Dynamo.Models.Get;
using Homedish.Aws.Dynamo.Models.Insert;
using Homedish.Aws.Dynamo.Models.Query;

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
