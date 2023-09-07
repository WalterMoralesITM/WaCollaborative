#region Using

using WaCollaborative.Shared.Responses;

#endregion Using

namespace WaCollaborative.Backend.Interfaces
{

    /// <summary>
    /// The Interface IGenericUnitOfWork
    /// </summary>

    public interface IGenericUnitOfWork<T> where T : class
    {

        #region Methods

        Task<Response<IEnumerable<T>>> GetAsync();

        Task<Response<T>> AddAsync(T model);

        Task<Response<T>> UpdateAsync(T model);

        Task<Response<T>> DeleteAsync(int id);

        Task<Response<T>> GetAsync(int id);

        #endregion Methods

    }
}