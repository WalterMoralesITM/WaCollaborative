#region Using

using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Data;

#endregion Using

namespace WaCollaborative.UnitTest.Shared
{
    /// <summary>
    /// The class ExceptionalDataContext
    /// </summary>

    public class ExceptionalDataContext : DataContext
    {

        #region Constructor

        public ExceptionalDataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }

        #endregion Constructor

        #region Methods

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Test Exception");
        }

        #endregion Methods

    }
}