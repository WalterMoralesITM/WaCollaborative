#region Using

using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Backend.Services;
using WaCollaborative.Shared.Entities;
using WaCollaborative.Shared.Enums;
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
        private readonly IUserHelper _userHelper;
        private readonly IFileStorage _fileStorage;

        #endregion Attributes

        #region Constructor

        public SeedDb(DataContext context, IApiService apiService, IUserHelper userHelper, IFileStorage fileStorage)
        {
            _context = context;
            _apiService = apiService;
            _userHelper = userHelper;
            _fileStorage = fileStorage;
        }

        #endregion Constructor

        #region Methods

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckSegmentsAsync();
            await CheckStatusType();
            await CheckStatus();
            await CheckMeasurementUnits();
            await CheckDistributionChannels();
            await CheckEventTypes();
            await CheckDemandTypes();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Efrain", "Trujillo", "truji@yopmail.com", "322 111 2222", "Avenida siempre viva 123", "EfrainTrujillo.jpeg", UserType.Planner);
            await CheckUserAsync("1020", "Jose", "Daza", "josedaza@yopmail.com", "313 644 9685", "Calle 5", "JoseDaza.jpeg", UserType.Planner);
            await CheckUserAsync("71469531", "Walter", "Morales", "wmorales@yopmail.com", "3053699685", "Calle 52 36 98", "Walter.jpeg", UserType.Planner);
            await CheckCollaborationCyclesAsync();
            //await CheckCollaborativeDemand();
        }

        private async Task CheckCollaborationCyclesAsync() {
            _context.CollaborationCycles.Add(new CollaborationCycle { Period = 202311,StatusId = 1 });
            _context.CollaborationCycles.Add(new CollaborationCycle { Period = 202312, StatusId = 1 });
            _context.CollaborationCycles.Add(new CollaborationCycle { Period = 202401, StatusId = 1 });
            _context.CollaborationCycles.Add(new CollaborationCycle { Period = 202402, StatusId = 1 });
            _context.CollaborationCycles.Add(new CollaborationCycle { Period = 202403, StatusId = 1 });
            _context.CollaborationCycles.Add(new CollaborationCycle { Period = 202404, StatusId = 1 });
          
            await _context.SaveChangesAsync();
        }
        private async Task CheckCollaborativeDemand()
        {
            // Consulta para obtener los datos necesarios
            var query = from p in _context.Portfolios
                        join pd in _context.PortfolioProducts on p.Id equals pd.PortfolioId
                        join cu in _context.PortfolioCustomers on p.Id equals cu.PortfolioId
                        join s in _context.ShippingPoints on cu.Id equals s.CustomerId
                        join c in _context.CollaborativeDemand on new { ProductId = pd.ProductId, ShippingPointId = s.Id } equals new { ProductId = c.ProductId, ShippingPointId = c.ShippingPointId } into temp
                        from cTemp in temp.DefaultIfEmpty()
                        where cTemp == null
                        select new
                        {
                            DemandTypeId = 1,
                            EventTypeId = 1,
                            pd.ProductId,
                            ShippingPointId = s.Id,
                            StatusId = 1
                        };

            // Insertar los datos en CollaborativeDemand si no existen
            foreach (var item in query)
            {
                _context.CollaborativeDemand.Add(new CollaborativeDemand
                {
                    DemandTypeId = item.DemandTypeId,
                    EventTypeId = item.EventTypeId,
                    ProductId = item.ProductId,
                    ShippingPointId = item.ShippingPointId,
                    StatusId = item.StatusId
                    // Agrega otras propiedades según sea necesario
                });
            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckDistributionChannels()
        {
            if (!_context.DistributionChannels.Any())
            {
                _context.DistributionChannels.Add(new DistributionChannel { Name = "DISTRIBUIDORES" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "CONSUMO INTERNO" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "PRESTADOR SERVICIOS PUBLICOS" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "INSTITUCIONAL" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "FORMATO DESCUENTOS" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "TRADICIONAL" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "INDUSTRIAL" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "MODERNO" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "CANAL ALTERNATIVO" });
                _context.DistributionChannels.Add(new DistributionChannel { Name = "GRANDES SUPERFICIES" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, string image, UserType userType)
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                if (city == null)
                {
                    city = await _context.Cities.FirstOrDefaultAsync();
                }

                string filePath;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    filePath = $"{Environment.CurrentDirectory}\\Images\\users\\{image}";
                }
                else
                {
                    filePath = $"{Environment.CurrentDirectory}/Images/users/{image}";
                }

                var fileBytes = File.ReadAllBytes(filePath);
                //var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = city,
                    UserType = userType//,
                    //Photo = imagePath,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Planner.ToString());
            await _userHelper.CheckRoleAsync(UserType.Collaborator.ToString());
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "PRODUCTOS ALIMENTICIOS" });
                _context.Categories.Add(new Category { Name = "PRODUCTOS DE ASEO" });
                _context.Categories.Add(new Category { Name = "QUIMICOS" });
                _context.Categories.Add(new Category { Name = "SALES" });
                _context.Categories.Add(new Category { Name = "ASEO" });
                _context.Categories.Add(new Category { Name = "CONDIMENTOS" });
                _context.Categories.Add(new Category { Name = "SAL" });
                _context.Categories.Add(new Category { Name = "SERVICIOS" });                
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckSegmentsAsync()
        {
            if (!_context.Segments.Any())
            {
                _context.Segments.Add(new Segment { Name = "ACIDO CLORHIDRICO" });
                _context.Segments.Add(new Segment { Name = "ACIDO SULFONICO" });
                _context.Segments.Add(new Segment { Name = "BLANQUEADORES" });
                _context.Segments.Add(new Segment { Name = "BOLSA ALTA PUREZA" });
                _context.Segments.Add(new Segment { Name = "ASEO" });
                _context.Segments.Add(new Segment { Name = "CONDIMENTOS" });
                _context.Segments.Add(new Segment { Name = "SAL" });
                _context.Segments.Add(new Segment { Name = "DESENGRASANTES" });
                _context.Segments.Add(new Segment { Name = "DESMANCHADOR" });
                _context.Segments.Add(new Segment { Name = "GOURMET" });
                await _context.SaveChangesAsync();
            }
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
                _context.MeasurementUnits.Add(new MeasurementUnit {Name = "BANDEJA" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "BIG BAG" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "BOLSA" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "BULTO" });                                                
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "CILINDRO" });                
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "DISPLAY" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "DOYPACK" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "GRANEL" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "MULTIPACK" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "SACO 50 KG" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "SACO 40 KG" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "SACO 25 KG" });
                _context.MeasurementUnits.Add(new MeasurementUnit { Name = "TERMOENCOGIBLE" });                          
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckEventTypes()
        {
            if (!_context.EventTypes.Any())
            {
                _context.EventTypes.Add(new EventType { Name = "Demada Regular" });
                _context.EventTypes.Add(new EventType { Name = "Lanzamiento" });                
                _context.EventTypes.Add(new EventType { Name = "Descuentos" });
                _context.EventTypes.Add(new EventType { Name = "Licitaciones" });
                _context.EventTypes.Add(new EventType { Name = "2x1" });
                _context.EventTypes.Add(new EventType { Name = "Madrugón" });
                _context.EventTypes.Add(new EventType { Name = "Trasnochón" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckDemandTypes()
        {
            if (!_context.DemandTypes.Any())
            {
                _context.DemandTypes.Add(new DemandType { Name = "Demanda Extraordinaria"});
                _context.DemandTypes.Add(new DemandType { Name = "Demanda Regular"});

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