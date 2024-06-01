using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class VwHotelsService
{
    public int HotelId { get; set; }

    public string HotelNameSys { get; set; }

    public int ServiceId { get; set; }

    public string ServiceNameSys { get; set; }

    public string ServiceIcon { get; set; }

    public bool? Status { get; set; }

    public int? Position { get; set; }

    public int ServiceContentId { get; set; }

    public string SeviceName { get; set; }

    public string LanguageName { get; set; }

    public string LanguageFlag { get; set; }

    public string LanguageAbbreviation { get; set; }

    public bool? LangStatus { get; set; }

    public string LanguageClass { get; set; }

    public int LangId { get; set; }
}
