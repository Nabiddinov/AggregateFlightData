using BizLogicLayer.Extensions;
using DataLayer;
using Microsoft.Extensions.Caching.Memory;
using SharedModel.Models;
using System.Linq.Dynamic.Core;

namespace BizLogicLayer.Services
{
    public class FlightAggregatorService : IFlightAggregatorService
    {
        private readonly IMemoryCache _cache;
        private readonly FlightDbContext _flightDbContext;
        private readonly Flight2DbContext _secondFlightDbContext;

        public FlightAggregatorService(IMemoryCache cache, FlightDbContext flightDbContext, Flight2DbContext secondFlightDbContext)
        {
            _cache = cache;
            _flightDbContext = flightDbContext;
            _secondFlightDbContext = secondFlightDbContext;
        }


        public IEnumerable<Flight> GetAggregatedFlights(FlightSearchCriteria criteria)
        {
            var cacheKey = $"Search_{criteria.Departure}_{criteria.Destination}_{criteria.DepartureDate?.ToString("yyyyMMdd")}_{criteria.MaxPrice}_{criteria.MaxLayovers}_{criteria.Airline}";

            if (_cache.TryGetValue(cacheKey, out List<Flight> cachedFlights))
            {
                return cachedFlights;
            }

            var source1Task = _flightDbContext.Flights.ApplyFilters(criteria).ToList();
            var source2Task = _secondFlightDbContext.Flights.ApplyFilters(criteria).ToList();

            var tasks = new[] { source1Task, source2Task };

            var allFlights = new List<Flight>();
            foreach (var task in tasks)
                allFlights.AddRange(task);

            _cache.Set(cacheKey, allFlights, TimeSpan.FromMinutes(10));

            return allFlights;
        }



        public string FlightCountMinimizer(int flightId, string passengerName, string passengerEmail)
        {
            var flight = _flightDbContext.Set<Flight>().Find(flightId); 

            if (flight != null)
            {
                if (flight.AvialablePlacesCount == 0)
                    return "Out";

                flight.AvialablePlacesCount--;

                _flightDbContext.Set<BookingRequest>().Add(new BookingRequest 
                { 
                    PassengerEmail = passengerEmail, 
                    PassengerName  = passengerName,
                    FlightId = flight.Id,
                });

                _flightDbContext.SaveChanges();

                return "Ok";
            }
            else
            {
                var flight2 = _secondFlightDbContext.Set<Flight>().Find(flightId);

                if (flight2 != null)
                {
                    if (flight2.AvialablePlacesCount == 0)
                        return "Out";

                    flight2.AvialablePlacesCount--;

                    _secondFlightDbContext.Set<BookingRequest>().Add(new BookingRequest
                    {
                        PassengerEmail = passengerEmail,
                        PassengerName = passengerName,
                        FlightId = flight.Id,
                    });

                    _flightDbContext.SaveChanges();

                    return "Ok";
                }
            }

            return "NotFound";
        }
    }
}

