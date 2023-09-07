#region Using

using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Responses;

#endregion Using

namespace WaCollaborative.Backend.UnitsOfWork
{

    /// <summary>
    /// The Class GenericUnitOfWork
    /// </summary>

    public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
    {

        #region Attributes

        private readonly IGenericRepository<T> _repository;

        #endregion Attributes

        #region Constructor

        public GenericUnitOfWork(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        #endregion Constructor

        #region Methods

        public async Task<Response<T>> AddAsync(T model) => await _repository.AddAsync(model);

        public async Task<Response<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<Response<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();

        public async Task<Response<T>> GetAsync(int id) => await _repository.GetAsync(id);

        public async Task<Response<T>> UpdateAsync(T model) => await _repository.UpdateAsync(model);

        #endregion Methods

    }
}