using System;
using System.Collections.Generic;

namespace HwaidakAPI.CheckInModels;

public partial class CheckInHotelsRoom
{
    public int RoomTypesId { get; set; }

    public int? HotelId { get; set; }

    public string RoomTypesEn { get; set; }
}
