using AutoMapper;
using HwaidakAPI.DTOs;
using HwaidakAPI.DTOs.Responses.Gyms;
using HwaidakAPI.DTOs.Responses.SPA;
using HwaidakAPI.Errors;
using HwaidakAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HwaidakAPI.Controllers
{
    public class SPAController : BaseApiController
    {
        private readonly HwaidakHotelsWsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public SPAController(HwaidakHotelsWsdbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }


        [HttpGet("{languageCode}/{hotelUrl}")]
        public async Task<ActionResult<GetSpaListPage>> GetHotelSPA(string hotelUrl, string languageCode = "en")
        {
            var hotel = await _context.VwHotels.Where(x => x.HotelUrl == hotelUrl && x.HotelStatus == true && x.LanguageAbbreviation == languageCode).FirstOrDefaultAsync();
            if (hotel == null) return NotFound(new ApiResponse(404, "there is no hotel with this name"));

            MainResponse pageDetails = new()
            {
                PageTitle = hotel.HotelWellnessTitle,
                PageText = hotel.HotelWellness,
                PageBannerPC = _configuration["ImagesLink"] + hotel.HotelWellnessBanner,
                PageBannerColorOverlayFrom = hotel.HotelWellnessBannerColorOverlayFrom,
                PageBannerColorOverlayTo = hotel.HotelWellnessBannerColorOverlayTo,
                PageBannerTablet = _configuration["ImagesLink"] + hotel.HotelWellnessBannerTablet,
                PageBannerTabletOverlayFrom = hotel.HotelWellnessBannerTabletColorOverlayFrom,
                PageBannerTabletOverlayTo = hotel.HotelWellnessBannerTabletColorOverlayTo,
                PageBannerMobile = _configuration["ImagesLink"] + hotel.HotelWellnessBannerMobile,
                PageBannerMobileOverlayFrom = hotel.HotelWellnessBannerMobileColorOverlayFrom,
                PageBannerMobileOverlayTo = hotel.HotelWellnessBannerMobileColorOverlayTo,
                PageMetatagTitle = hotel.HotelWellnessMetatagTitle,
                PageMetatagDescription = hotel.HotelWellnessMetatagDescription
            };

            var spas = await _context.VwSpas.Where(x => x.HotelUrl == hotelUrl && x.LanguageAbbreviation == languageCode && x.FacilityStatus == true && x.IsDeleted == false).OrderBy(x => x.FacilityPosition).OrderBy(x => x.FacilityPosition).ToListAsync();

            var spaDto = _mapper.Map<List<GetSPA>>(spas);

            foreach (var item in spaDto)
            {
                item.FacilityPhoto = _configuration["ImagesLink"] + item.FacilityPhoto;
            }

            GetSpaListPage model = new()
            {
                PageDetails = pageDetails,
                sPAs = spaDto
            };


            return Ok(model);
        }
        [HttpGet("{languageCode}/{hotelUrl}/{SPAUrl}")]
        public async Task<ActionResult<IEnumerable<GetSPADetails>>> GetSPA(string hotelUrl,  string SPAUrl, string languageCode = "en")
        {
            var spa = await _context.VwSpas.Where(x => x.HotelUrl == hotelUrl && x.IsDeleted==false && x.FacilityUrl == SPAUrl && x.LanguageAbbreviation == languageCode  && x.FacilityStatus == true).FirstOrDefaultAsync();
            if (spa == null) return NotFound(new ApiResponse(404, "this SPA doesnt exist"));
            var spaDto = _mapper.Map<GetSPADetails>(spa);
            var spaGallery = await _context.TblSpaGalleries.Where(x => x.Spaid == spa.SpaId &&x.PhotoStatus==true).OrderBy(x => x.PhotoPosition).ToListAsync();
            var spaServices = await _context.VwSpaServices.Where(x => x.LanguageAbbreviation == languageCode && x.SpaId == spa.SpaId &&x.SpaservicesStatus==true && x.LanguageAbbreviation == languageCode).OrderBy(x => x.SpaservicesPosition).ToListAsync();

            spaDto.FacilityPhoto = _configuration["ImagesLink"] + spaDto.FacilityPhoto;
            spaDto.FacilityBanner = _configuration["ImagesLink"] + spaDto.FacilityBanner;
            spaDto.FacilityBannerMobile = _configuration["ImagesLink"] + spaDto.FacilityBannerMobile;
            spaDto.FacilityBannerTablet = _configuration["ImagesLink"] + spaDto.FacilityBannerTablet;

            spaDto.SPAGallery = spaGallery != null ? _mapper.Map<List<GetSPAGallery>>(spaGallery) : null;
            foreach (var item in spaDto.SPAGallery)
            {
                item.PhotoFile = _configuration["ImagesLink"] + item.PhotoFile;
            }
            spaDto.SPAServices = spaServices != null ? _mapper.Map<List<GetSPAService>>(spaServices) : null;

            return Ok(spaDto);
        }

    }
}
