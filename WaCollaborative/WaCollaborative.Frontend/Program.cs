using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Localization;
using MudBlazor;
using MudBlazor.Services;
using OfficeOpenXml;

using WaCollaborative.Frontend;
using WaCollaborative.Frontend.Auth;
using WaCollaborative.Frontend.Helpers;
using WaCollaborative.Frontend.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://wacollaborativebackend20231118171350.azurewebsites.net/") });
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddBlazoredModal();
builder.Services.AddMudServices();
//builder.Services.AddLocalization();
builder.Services.AddTransient<MudLocalizer, DictionaryMudLocalizer>();

builder.Services.AddSingleton<ExcelExporter>();
//builder.Services.AddHttpClient();

await builder.Build().RunAsync();