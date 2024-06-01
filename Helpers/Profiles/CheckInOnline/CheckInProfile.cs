using AutoMapper;
using HwaidakAPI.CheckInModels;
using HwaidakAPI.DTOs.Responses.CheckInOnline;

namespace HwaidakAPI.Helpers.Profiles.CheckInOnline
{
    public class CheckInProfile : Profile
    {
        public CheckInProfile()
        {
            CreateMap<CheckInHotel, CheckInHotels> ();
            CreateMap<CheckInReservationThrough, CheckInReservationth>();
        }
    }
}
