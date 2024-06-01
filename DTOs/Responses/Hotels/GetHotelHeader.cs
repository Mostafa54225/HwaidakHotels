using HwaidakAPI.DTOs.Responses.Home;

namespace HwaidakAPI.DTOs.Responses.Hotels
{
    public class GetHotelHeader
    {
        public string HotelPhone { get; set; }
        public string HotelEmail { get; set; }
        public string HotelLogo { get; set; }
        public string HotelLogoColored { get; set; }
        public string ButtonGroupUrl { get; set; }
        public bool? IsEnableFaq { get; set; }

        public bool? IsEnableOffer { get; set; }

        public bool? IsEnableSpa { get; set; }

        public bool? IsEnableAwards { get; set; }

        public bool? IsEnableLocation { get; set; }

        public bool? IsEnableAllInclusive { get; set; }

        public bool? IsEnableWedding { get; set; }

        public bool? IsEnableNews { get; set; }

        public bool? IsEnableHoneyMooners { get; set; }

        public bool? IsEnableTestimonials { get; set; }

        public List<DiningTypes> DiningTypes { get; set; } = [];

        public List<LanguageResponse> Languages { get; set; } = [];
    }
}

public class DiningTypes
{
    public string RestaurantsTypeName { get; set; }
    public string FilterBy { get; set; }
    public string HotelUrl { get; set; }
}