using SharedModel.Models;

namespace BizLogicLayer.Services
{
    using DataLayer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using System.Threading;

    public interface IFlightAggregatorService
    {
        IEnumerable<Flight> GetAggregatedFlights(FlightSearchCriteria criteria);
        string FlightCountMinimizer(int flightId, string passengerName, string passengerEmail);
    }
}
