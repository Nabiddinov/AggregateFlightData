using SharedModel.Models;

namespace BizLogicLayer.Services
{
    public class BookingService : IBookingService
    {
        private readonly IFlightAggregatorService _flightAggregatorService;

        public BookingService(IFlightAggregatorService flightAggregatorService)
        {
            _flightAggregatorService = flightAggregatorService;
        }


        public string BookFlight(BookingRequestDto request)
        {
            var flights = _flightAggregatorService.GetAggregatedFlights(new FlightSearchCriteria
            {
                FlightNumber = request.FlightNumber
            });

            var flight = flights.FirstOrDefault();

            if (flight == null)
                return "NotFount";

            return  _flightAggregatorService.FlightCountMinimizer(flight.Id, request.PassengerName, request.PassengerEmail);
        }
    }
}
