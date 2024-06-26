﻿namespace HwaidakAPI.DTOs.Responses.Restaurants
{
    public class GetRestaurantDetails
    {
        public string RestaurantName { get; set; }
        public string RestaurantPhoto { get; set; }
        public string RestaurantUrl { get; set; }
        public string HotelUrl { get; set; }
        public string MetatagTitle { get; set; }
        public string MetatagDescription { get; set; }
        public string RestaurantBanner { get; set; }
        public string RestaurantBannerColorOverlayTo { get; set; }
        public string RestaurantBannerColorOverlayFrom { get; set; }
        public string RestaurantBannerTablet { get; set; }
        public string RestaurantBannerTabletColorOverlayFrom { get; set; }
        public string RestaurantBannerTabletColorOverlayTo { get; set; }
        public string RestaurantBannerMobile { get; set; }
        public string RestaurantBannerMobileColorOverlayFrom { get; set; }
        public string RestaurantBannerMobileColorOverlayTo { get; set; }
        public string DressCode { get; set; }
        public string RestaurantCuisine { get; set; }
        public string RestaurantOpenFor { get; set; }
        public string RestaurantMenu { get; set; }
        public string Openinghours { get; set; }
        public string RestaurantNote { get; set; }
        public string RestaurantSummery { get; set; }
        public string RestaurantDetails { get; set; }
        public bool? IsMoreDetails { get; set; }

        public List<GetRestaurantGallery> RestaurantGalleries { get; set; }
        //public GetRestaurantType GetRestaurantType { get; set; }
        public List<GetRestaurant> OtherRestaurants { get; set; }
    }
}
