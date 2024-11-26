using SharedModel.Models;

namespace BizLogicLayer.Services
{
    public interface IBookingService
    {
        string BookFlight(BookingRequestDto request);
    }
}
