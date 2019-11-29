using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.WebRequestAccessLayer
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token = "");

        Task<TResult> Delete<TResult>(string id);
    }
}
