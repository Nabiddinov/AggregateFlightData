using BizLogicLayer.Services;
using BlazorClient;
using DataLayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<IFlightAggregatorService, FlightAggregatorService>();
builder.Services.AddScoped<BookingService>();

builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FirstSource")));
builder.Services.AddDbContext<Flight2DbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SecondSource")));

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress.Replace("BlazorClient", "AggregateFlightData"))
});

builder.Logging.ClearProviders();
//builder.Logging.AddConsole();
builder.Services.AddMemoryCache();
await builder.Build().RunAsync();
