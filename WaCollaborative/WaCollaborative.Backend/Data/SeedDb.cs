#region Using

using WaCollaborative.Shared.Entities;

#endregion Using

namespace WaCollaborative.Backend.Data
{

    /// <summary>
    /// The Class SeedDb
    /// </summary>

    public class SeedDb
    {

        #region Attributes

        private readonly DataContext _context;

        #endregion Attributes

        #region Constructor

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Methods

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckMeasurementUnits();
        }

        private async Task CheckMeasurementUnits()
        {
            if (!_context.MeasurementUnits.Any())
            {
                _context.MeasurementUnits.Add(new MeasurementUnit {Name = "Tonelada"});
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Kilogramo" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Libra" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Gramo" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Onza" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Caja" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Bulto" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Estiba" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Lote" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Metro Cúbico" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "Litro" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>() {
                    new State()
                    {
                        Name = "Antioquia",
                        Cities = new List<City>() {
                            new City() { Name = "Medellín" },
                            new City() { Name = "Itagüí" },
                            new City() { Name = "Envigado" },
                            new City() { Name = "Bello" },
                            new City() { Name = "Rionegro" },
                        }
                    },
                    new State() {
                        Name = "Bogotá",
                        Cities = new List<City>() {
                            new City() { Name = "Usaquen" },
                            new City() { Name = "Champinero" },
                            new City() { Name = "Santa fe" },
                            new City() { Name = "Useme" },
                            new City() { Name = "Bosa" },
                        }
                    },
                }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>() {
                    new State()
                    {
                        Name = "Florida",
                        Cities = new List<City>() {
                            new City() { Name = "Orlando" },
                            new City() { Name = "Miami" },
                            new City() { Name = "Tampa" },
                            new City() { Name = "Fort Lauderdale" },
                            new City() { Name = "Key West" },
                        }
                    },
                    new State()
                    {
                        Name = "Texas",
                        Cities = new List<City>() {
                            new City() { Name = "Houston" },
                            new City() { Name = "San Antonio" },
                            new City() { Name = "Dallas" },
                            new City() { Name = "Austin" },
                            new City() { Name = "El Paso" },
                        }
                    },
                }
                });
            }

            await _context.SaveChangesAsync();
        }

        #endregion Methods

    }
}