using System;
using System.Collections.Generic;

namespace HwaidakAPI.CheckInModels;

public partial class CheckInHotel
{
    public string HotelName { get; set; }

    public int HotelId { get; set; }

    public int? HotelPosition { get; set; }

    public string HotelReceivedEmail { get; set; }

    public string HotelTermsConditions { get; set; }

    public bool? HotelStatus { get; set; }
}
