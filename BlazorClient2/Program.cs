using BizLogicLayer.Services;
using BlazorClient2.Components;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Serilog.Sinks.PostgreSQL;

namespace BlazorClient2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.PostgreSQL(
                    connectionString: builder.Configuration.GetConnectionString("FirstSource"),
                    tableName: "logs",
                    columnOptions: ColumnOptions.Default,
                    needAutoCreateTable: true
                    )
                .CreateLogger();

            builder.Logging.AddSerilog();


            // Add services to the container.
            builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

            builder.Services.AddDbContext<FlightDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("FirstSource")));
            builder.Services.AddDbContext<Flight2DbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("SecondSource")));

            builder.Services.AddScoped<HttpClient>();
            builder.Services.AddScoped<IMemoryCache, MemoryCache>();
            builder.Services.AddScoped<IFlightAggregatorService, FlightAggregatorService>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
