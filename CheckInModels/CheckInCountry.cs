using System;
using System.Collections.Generic;

namespace HwaidakAPI.CheckInModels;

public partial class CheckInCountry
{
    public int? Langid { get; set; }

    public string RequestCountryName { get; set; }

    public int RequestCountryId { get; set; }

    public string RequestCountryNameFr { get; set; }

    public string RequestCountryNameAr { get; set; }
}
