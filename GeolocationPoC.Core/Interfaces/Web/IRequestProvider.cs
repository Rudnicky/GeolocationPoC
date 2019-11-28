using System.Threading.Tasks;

namespace GeolocationPoC.Core.Interfaces.Web
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri, string token);
    }
}
