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

        public Task<IEnumerable<T>> GetAsync();

        public Task<T> GetAsync(int id);

        public Task<Response<T>> AddAsync(T model);

        public Task<Response<T>> UpdateAsync(T model);

        public Task DeleteAsync(int id);

        #endregion Methods

    }
}