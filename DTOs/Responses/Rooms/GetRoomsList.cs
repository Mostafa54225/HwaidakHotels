using HwaidakAPI.DTOs.Responses.Hotels;

namespace HwaidakAPI.DTOs.Responses.Rooms
{
    public class GetRoomsList
    {
        public MainResponse PageDetails { get; set; }
        public List<GetRoomsPageList> RoomsList { get; set; }
    }
}
