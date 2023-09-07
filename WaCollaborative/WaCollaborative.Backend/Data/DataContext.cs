#region Using

using Microsoft.EntityFrameworkCore;
using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.Backend.Data
{

    /// <summary>
    /// The Class DataContext
    /// </summary>

    public class DataContext : DbContext
    {

        #region Constructor

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #endregion Constructor

        #region Entities

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        #endregion Entities

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();           
            modelBuilder.Entity<State>().HasIndex(s => new { s.Name, s.CountryId }).IsUnique();
            modelBuilder.Entity<City>().HasIndex(c => new { c.Name, c.StateId }).IsUnique();
        }

        #endregion Methods

    }
}