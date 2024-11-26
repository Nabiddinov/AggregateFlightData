namespace SharedModel.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public int Layovers { get; set; }
        public int TotalPlacesCount { get; set; }
        public int AvialablePlacesCount { get; set; }
    }
}
