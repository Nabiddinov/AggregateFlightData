using SharedModel.Models;

namespace BizLogicLayer.Extensions
{
    public static class FlightExtensions
    {
        public static IEnumerable<Flight> ApplyFilters(this IEnumerable<Flight> flights, FlightSearchCriteria criteria)
        {
            if (!string.IsNullOrEmpty(criteria.Departure))
                flights = flights.Where(f => f.Departure == criteria.Departure);

            if (!string.IsNullOrEmpty(criteria.Destination))
                flights = flights.Where(f => f.Destination == criteria.Destination);

            if (criteria.DepartureDate.HasValue)
                flights = flights.Where(f => f.DepartureTime.Date == criteria.DepartureDate.Value.Date);

            if (criteria.MaxPrice.HasValue)
                flights = flights.Where(f => f.Price <= criteria.MaxPrice.Value);

            if (criteria.MaxLayovers.HasValue)
                flights = flights.Where(f => f.Layovers <= criteria.MaxLayovers.Value);

            if (!string.IsNullOrEmpty(criteria.Airline))
                flights = flights.Where(f => f.Airline == criteria.Airline);

            return flights;
        }
    }
}
