#region Using

using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Services;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Responses;

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
        private readonly IApiService _apiService;

        #endregion Attributes

        #region Constructor

        public SeedDb(DataContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
        }

        #endregion Constructor

        #region Methods

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckStatusType();
            await CheckStatus();
            await CheckMeasurementUnits();
        }

        private async Task CheckStatusType() 
        {
            if (!_context.StatusType.Any()) 
            {
                _context.StatusType.Add(new StatusType { Name = "Paramétrico" });
                _context.StatusType.Add(new StatusType { Name = "Proceso" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckStatus()
        {
            if (!_context.Status.Any())
            {
                _context.Status.Add(new Status { Name = "Activo", StatusTypeId = 1 });
                _context.Status.Add(new Status { Name = "Inactivo", StatusTypeId = 1 });
                _context.Status.Add(new Status { Name = "Creado", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Parametrizado", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Elaboración", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Pendiente Por Aprobación", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Aprobado", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Terminado", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Archivado", StatusTypeId = 2 });
                _context.Status.Add(new Status { Name = "Anulado", StatusTypeId = 2 });
                await _context.SaveChangesAsync();
            }
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
                var responseCountries = await _apiService.GetAsync<List<CountryResponse>>("/v1", "/countries");
                if (responseCountries.WasSuccess)
                {
                    var countries = responseCountries.Result!;
                    foreach (var countryResponse in countries)
                    {
                        var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            var responseStates = await _apiService.GetAsync<List<StateResponse>>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.WasSuccess)
                            {
                                var states = responseStates.Result!;
                                foreach (var stateResponse in states!)
                                {
                                    var state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        var responseCities = await _apiService.GetAsync<List<CityResponse>>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.WasSuccess)
                                        {
                                            var cities = responseCities.Result!;
                                            foreach (var cityResponse in cities)
                                            {
                                                if (cityResponse.Name == "Mosfellsbær" || cityResponse.Name == "Șăulița")
                                                {
                                                    continue;
                                                }
                                                var city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            country.States.Add(state);
                                        }
                                    }
                                }
                            }
                            if (country.StatesNumber > 0)
                            {
                                _context.Countries.Add(country);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }

        #endregion Methods

    }
}