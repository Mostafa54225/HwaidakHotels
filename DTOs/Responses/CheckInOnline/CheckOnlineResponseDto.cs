namespace HwaidakAPI.DTOs.Responses.CheckInOnline
{
    public class CheckOnlineResponseDto
    {
        public MainResponse PageDetails { get; set; }
        public List<CheckInHotels> CheckInHotels { get; set; }
        public List<CheckInReservationth> CheckInReservationThroughs { get; set; }
    }
}
