using System.Threading.Tasks;
using Homedish.Dynamo.Models;
using Homedish.Dynamo.Models.Delete;
using Homedish.Dynamo.Models.Get;
using Homedish.Dynamo.Models.Insert;
using Homedish.Dynamo.Models.Query;

namespace Homedish.Dynamo
{
    public interface IOperations
    {
        Task<InsertResponseModel> InsertAsync(InsertModel value);
        Task<GetResponseModel> GetAsync(GetModel value);
        Task<QueryResponseModel> QueryAsync(QueryModel value);
        Task<DeleteResponseModel> DeleteAsync(DeleteModel value);
    }
}
