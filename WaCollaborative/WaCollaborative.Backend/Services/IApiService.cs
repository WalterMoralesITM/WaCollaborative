#region Using

using WaCollaborative.Shared.Responses;

#endregion Using

namespace WaCollaborative.Backend.Services
{

    /// <summary>
    /// The interface IApiService
    /// </summary>

    public interface IApiService
    {

        #region Methods

        public Task<Response<T>> GetAsync<T>(string servicePrefix, string controller);

        #endregion Methods

    }
}