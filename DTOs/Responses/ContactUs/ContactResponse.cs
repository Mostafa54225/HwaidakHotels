using HwaidakAPI.DTOs.Responses.Hotels;

namespace HwaidakAPI.DTOs.Responses.ContactUs
{
    public class ContactResponse
    {
        public MainResponse PageDetails { get; set; }
        public GetContactHotel ContactDetails { get; set; }
        public List<GetHotelNearBy> HotelNearBy { get; set; }
    }
}
