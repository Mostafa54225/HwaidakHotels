using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class VwHotelRestaurantTypesActive
{
    public string HotelUrl { get; set; }

    public int? HotelId { get; set; }

    public string FilterBy { get; set; }

    public int RestaurantsTypeId { get; set; }

    public string RestaurantsTypeName { get; set; }
}
