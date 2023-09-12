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

        public DbSet<Country> Countries { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<StatusType> StatusType { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }

        #endregion Entities

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();           
            modelBuilder.Entity<State>().HasIndex(s => new { s.Name, s.CountryId }).IsUnique();
            modelBuilder.Entity<City>().HasIndex(c => new { c.Name, c.StateId }).IsUnique();
            modelBuilder.Entity<StatusType>().HasIndex(s => s.Name).IsUnique();
            modelBuilder.Entity<Status>().HasIndex(s => new { s.Name, s.StatusTypeId }).IsUnique();
            modelBuilder.Entity<MeasurementUnit>().HasIndex(m => m.Name).IsUnique();
        }

        #endregion Methods

    }
}