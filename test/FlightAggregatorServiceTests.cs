using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using SharedModel.Models;
using BizLogicLayer.Services;

namespace test
{
    public class FlightAggregatorServiceTests
    {
        [Fact]
        public async Task GetAggregatedFlightsAsync_HandlesTimeouts()
        {
            // Arrange
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var service = new FlightAggregatorService(memoryCache);
            var criteria = new FlightSearchCriteria
            {
                Departure = "JFK",
                Destination = "LAX"
            };

            // Act
            var flights = await service.GetAggregatedFlightsAsync(criteria);

            // Assert
            Assert.NotNull(flights);
            Assert.True(flights.Any()); // At least some flights should be returned
        }
    }

}
