namespace SharedModel.Models
{
    public class BookingRequest
    {
        public int Id { get; set; }
        public string PassengerName { get; set; }
        public string PassengerEmail { get; set; }
        public int FlightId { get; set; }
        public Flight Flight { get; set; }
    }
}
