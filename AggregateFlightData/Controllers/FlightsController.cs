using SharedModel.Models;
using BizLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AggregateFlightData.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightAggregatorService _flightAggregator;
        private readonly ILogger<FlightsController> _logger;

        public FlightsController(ILogger<FlightsController> logger, IFlightAggregatorService flightAggregator)
        {
            _flightAggregator = flightAggregator;
            _logger = logger;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchFlightsAsync([FromBody] FlightSearchCriteria criteria)
        {
            _logger.LogInformation("Search request received: {@Criteria}", criteria);

            try
            {
                var flights = await _flightAggregator.GetAggregatedFlightsAsync(criteria);
                _logger.LogInformation("Search completed. Results count: {Count}", flights.Count());
                return Ok(flights);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while searching flights");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookFlight([FromBody] BookingRequest request)
        {
            _logger.LogInformation("Booking request received for flight: {FlightNumber}", request.FlightNumber);

            try
            {
                var bookingService = new BookingService();
                await bookingService.InitializeAsync();
                var result = bookingService.BookFlight(request);

                if (result)
                {
                    _logger.LogInformation("Booking successful for flight: {FlightNumber}", request.FlightNumber);
                    return Ok(new { Message = "Booking successful" });
                }
                else
                {
                    _logger.LogWarning("Booking failed for flight: {FlightNumber}", request.FlightNumber);
                    return BadRequest(new { Message = "Booking failed. Flight not found or already booked." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while booking the flight");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }

}
