using WaCollaborative.Shared.Responses;

namespace WaCollaborative.Backend.Services
{
    public interface IApiService
    {
        Task<Response<T>> GetAsync<T>(string servicePrefix, string controller);
    }
}
