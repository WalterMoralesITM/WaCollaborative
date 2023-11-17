#region Using

using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;
using WaCollaborative.Backend.Interfaces;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Responses;

#endregion Using

namespace WaCollaborative.Backend.Repositories
{
    /// <summary>
    /// The Class GenericRepository
    /// </summary>

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        #region Attributes

        private readonly DataContext _context;
        private readonly DbSet<T> _entity;

        #endregion Attributes

        #region Constructor

        public GenericRepository(DataContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        #endregion Constructor

        #region Methods

        public async Task<Response<T>> AddAsync(T entity)
        {
            _context.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new Response<T>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException dbUpdateException)
            {
                return DbUpdateExceptionResponse(dbUpdateException);
            }
            catch (Exception exception)
            {
                return ExceptionResponse(exception);
            }
        }

        public async Task<Response<T>> DeleteAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row != null)
            {
                _entity.Remove(row);
                await _context.SaveChangesAsync();
                return new Response<T>
                {
                    WasSuccess = true,
                };
            }
            return new Response<T>
            {
                WasSuccess = false,
                Message = "Registro no encontrado"
            };
        }

        public async Task<T> GetAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            return row!;
        }

        public async Task<IEnumerable<T>> GetAsync()
        {
            return await _entity.ToListAsync();
        }

        public async Task<Response<T>> UpdateAsync(T entity)
        {
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return new Response<T>
                {
                    WasSuccess = true,
                    Result = entity
                };
            }
            catch (DbUpdateException dbUpdateException)
            {
                return DbUpdateExceptionResponse(dbUpdateException);
            }
            catch (Exception exception)
            {
                return ExceptionResponse(exception);
            }
        }

        private Response<T> ExceptionResponse(Exception exception)
        {
            return new Response<T>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }

        private Response<T> DbUpdateExceptionResponse(DbUpdateException dbUpdateException)
        {
            if (dbUpdateException.InnerException!.Message.Contains("duplicate"))
            {
                return new Response<T>
                {
                    WasSuccess = false,
                    Message = "Ya existe el registro que estas intentando crear."
                };
            }
            else
            {
                return new Response<T>
                {
                    WasSuccess = false,
                    Message = dbUpdateException.InnerException.Message
                };
            }
        }

        public async Task<Country> GetCountryAsync(int id)
        {
            var country = await _context.Countries
                    .Include(c => c.States!)
                    .ThenInclude(s => s.Cities)
                    .FirstOrDefaultAsync(c => c.Id == id);
            return country!;
        }

        public async Task<State> GetStateAsync(int id)
        {
            var state = await _context.States
                    .Include(s => s.Cities)
                    .FirstOrDefaultAsync(c => c.Id == id);
            return state!;
        }

        public async Task<Portfolio> GetPortfolioAsync(int id)
        {
            var portfolio = await _context.Portfolios
                   .Include(c => c.PortfolioCustomers!)
                   .FirstOrDefaultAsync(c => c.Id == id);
            return portfolio!;
        }

        public async Task<PortfolioCustomer> GetPortfolioCustomerAsync(int id)
        {
            var portfoliosCustomer = await _context.PortfolioCustomers
                    .FirstOrDefaultAsync(c => c.Id == id);
            return portfoliosCustomer!;
        }

        #endregion Methods

    }
}