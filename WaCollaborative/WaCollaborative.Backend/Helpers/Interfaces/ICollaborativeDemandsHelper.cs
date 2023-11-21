using WaCollaborative.Shared.Responses;

namespace WaCollaborative.Backend.Helpers.Interfaces
{
    public interface ICollaborativeDemandsHelper
    {
        Task<Response<bool>> SynchronizeCollaborativeDemandsAsync();
    }
}
