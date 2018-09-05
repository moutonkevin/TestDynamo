using System.Threading.Tasks;

namespace Homedish.Template.Core.Services
{
    public interface ITestService
    {
        Task<string> Get(int id);
    }
}