using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class TblHotelsService
{
    public int ServiceId { get; set; }

    public int? HotelId { get; set; }

    public string ServiceNameSys { get; set; }

    public string ServiceIcon { get; set; }

    public bool? Status { get; set; }

    public int? Position { get; set; }

    public virtual Hotel Hotel { get; set; }

    public virtual ICollection<TblHotelsServicesContent> TblHotelsServicesContents { get; set; } = new List<TblHotelsServicesContent>();
}
