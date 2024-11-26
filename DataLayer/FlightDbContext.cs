using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Logging.Abstractions;
using SharedModel.Models;

namespace DataLayer
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) { }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<BookingRequest> Bookings { get; set; }

        public static void SeedData(MigrationBuilder migrationBuilder)
        {
            var random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var totalPlacesCount = random.Next(50, 200);

                migrationBuilder.InsertData(
                    table: "Flights",
                    columns: new[] { "FlightNumber", "Airline", "Departure", "Destination", "DepartureTime", "ArrivalTime", "Price", "Layovers", "TotalPlacesCount", "AvialablePlacesCount" },
                    values: new object[]
                    {
                    $"FL{random.Next(100, 999)}-{random.Next(1000, 9999)}",
                    new[] { "American Airlines", "Delta", "United", "JetBlue", "Southwest Airlines" }[random.Next(5)],
                    new[] { "New York", "Los Angeles", "Chicago", "London", "Paris", "Tokyo", "Berlin" }[random.Next(7)],
                    new[] { "New York", "Los Angeles", "Chicago", "London", "Paris", "Tokyo", "Berlin" }[random.Next(7)],
                    DateTime.Now.AddDays(random.Next(0, 365)).ToUniversalTime(),
                    DateTime.Now.AddDays(random.Next(1, 365)).ToUniversalTime(),
                    random.Next(50, 1500) + random.NextDouble(),
                    random.Next(0, 3),
                    totalPlacesCount,
                    totalPlacesCount
                    });
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
     .Property(f => f.DepartureTime)
     .HasConversion(
         v => v.ToUniversalTime(),
         v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            modelBuilder.Entity<Flight>()
        .Property(f => f.ArrivalTime)
        .HasConversion(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
        }
    }
}
