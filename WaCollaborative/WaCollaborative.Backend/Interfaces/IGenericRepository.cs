#region Using

using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Responses;

#endregion Using

namespace WaCollaborative.Backend.Interfaces
{

    /// <summary>
    /// The Interface IGenericRepository
    /// </summary>

    public interface IGenericRepository<T> where T : class
    {

        #region Methods

        public Task<T> GetAsync(int id);

        public Task<IEnumerable<T>> GetAsync();

        public Task<Response<T>> AddAsync(T entity);

        public Task<Response<T>> UpdateAsync(T entity);

        public Task<Response<T>> DeleteAsync(int id);

        public Task<Country> GetCountryAsync(int id);

        public Task<State> GetStateAsync(int id);

        public Task<Portfolio> GetPortfolioAsync(int id);

        public Task<PortfolioCustomer> GetPortfolioCustomerAsync(int id);

        #endregion Methods

    }
}