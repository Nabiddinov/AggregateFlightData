namespace SharedModel.Models
{
    public class FlightSearchCriteria
    {
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime? DepartureDate { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MaxLayovers { get; set; }
        public string Airline { get; set; }
        public string FlightNumber { get; set; }
    }

}
