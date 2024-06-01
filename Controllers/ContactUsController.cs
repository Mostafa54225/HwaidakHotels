using AutoMapper;
using HwaidakAPI.DTOs.Responses.Restaurants;
using HwaidakAPI.DTOs;
using HwaidakAPI.Errors;
using HwaidakAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HwaidakAPI.DTOs.Responses.ContactUs;
using System;
using HwaidakAPI.OPModels;
using OrientHGAPI.DTOs.Responses.ContactUs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrientHGAPI.Helpers;
using HwaidakAPI.DTOs.Requests;
using HwaidakAPI.DTOs.Responses.Hotels;

namespace HwaidakAPI.Controllers
{
    public class ContactUsController : BaseApiController
    {
        private readonly HwaidakHotelsWsdbContext _context;
        private readonly HwaidakHotelsOpedbContext _opContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _email;

        public ContactUsController(HwaidakHotelsWsdbContext context, HwaidakHotelsOpedbContext opContext, IMapper mapper, IConfiguration configuration, IEmailSender email)
        {
            _context = context;
            _opContext = opContext;
            _mapper = mapper;
            _configuration = configuration;
            _email = email;
        }

        [HttpGet("{languageCode}/{hotelUrl}")]
        public async Task<ActionResult<ContactResponse>> GetHotelContactUs(string hotelUrl, string languageCode = "en")
        {
            var hotel = await _context.VwHotels.Where(x => x.HotelUrl == hotelUrl && x.HotelStatus == true && x.LanguageAbbreviation == languageCode).FirstOrDefaultAsync();
            if (hotel == null) return NotFound(new ApiResponse(404, "there is no hotel with this name"));
            
            MainResponse pagedetails = new MainResponse
            {
                PageTitle = hotel.HotelContactTitle,
                PageBannerPC = _configuration["ImagesLink"] + hotel.HotelContactBanner,
                PageBannerColorOverlayFrom = hotel.HotelContactBannerColorOverlayFrom,
                PageBannerColorOverlayTo = hotel.HotelContactBannerColorOverlayTo,
                PageBannerMobile = _configuration["ImagesLink"] + hotel.HotelContactBannerMobile,
                PageBannerMobileOverlayFrom = hotel.HotelContactBannerMobileColorOverlayFrom,
                PageBannerMobileOverlayTo = hotel.HotelContactBannerMobileColorOverlayTo,
                PageBannerTablet = _configuration["ImagesLink"] + hotel.HotelContactBannerTablet,
                PageBannerTabletOverlayFrom = hotel.HotelContactBannerTabletColorOverlayFrom,
                PageBannerTabletOverlayTo = hotel.HotelContactBannerTabletColorOverlayTo,
                PageText = hotel.HotelContact,
                PageMetatagTitle = hotel.HotelContactMetaTagTitle,
                PageMetatagDescription = hotel.HotelContactMetatagDescription
            };
            var hotelNearBy = await _context.VwHotelsNearBies.Where(x => x.HotelId == hotel.HotelId && x.LanguageAbbreviation == languageCode && x.HotelNearByStatus == true).ToListAsync();
            var contactsDto = _mapper.Map<GetContactHotel>(hotel);



            ContactResponse model = new()
            {
                PageDetails = pagedetails,
                ContactDetails = contactsDto,
                HotelNearBy = hotelNearBy != null ? _mapper.Map<List<GetHotelNearBy>>(hotelNearBy) : null
            };


            return Ok(model);
        }



        [HttpPost("ContactUs-Submit")]
        public async Task<ActionResult<TblRequestsContact>> CreateContactUs(ContactUsRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(y => y.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(errors);
            }

            var entity = new TblRequestsContact
            {

                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                CustomerPhone = request.CustomerPhone,
                CustomerMessage = request.CustomerMessage
            };


            var item = await _opContext.TblRequestsContacts.AddAsync(entity);
            await _opContext.SaveChangesAsync();
            if (item == null)
                return BadRequest("ContactUs not save to DB");




            #region Send Mail
            string body =
                @"<table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                       <tbody>";



            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Customer Name: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.CustomerName + @" </td>
                       </tr>";




            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Customer Email: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.CustomerEmail + @" </td>
                       </tr>";

            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Customer Mobile: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.CustomerPhone + @" </td>
                       </tr>";
            body +=
           @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Customer Email: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.CustomerEmail + @" </td>
                       </tr>";



            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Message: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.CustomerMessage + @" </td>
                       </tr>";
            body += @"</tbody></table> ";
            #endregion



            try
            {
                //_email.SendMail("do_not_reply@hwaidakhotels.com", entity.CustomerEmail, "ContactUs Request", body);
                //_email.SendMail("do_not_reply@hwaidakhotels.com", "ahmed.taha@titegypt.com", "ContactUs Request", body);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return Ok(ex.Message);
            }
        }


        [HttpPost("Newsletter-Submit")]
        public async Task<ActionResult> CreateNewsLetterRequest(string request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(y => y.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(errors);
            }

            var entity = new TblRequestsNewsletter
            {
                Emailaddress = request
            };


            var item = await _opContext.TblRequestsNewsletters.AddAsync(entity);
            await _opContext.SaveChangesAsync();
            if (item == null)
                return BadRequest("ContactUs not save to DB");




            #region Send Mail
            string body =
            @"<h3>NewsLetter Request</h3><table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                       <tbody>";


            body +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Customer Email: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.Emailaddress + @" </td>
                       </tr>";


            body += @"</tbody>
        </table>
    ";
            #endregion





            try
            {
                //_email.SendMail("do_not_reply@hwaidakhotels.com", entity.Emailaddress, "Newsletter Request", body);
                //_email.SendMail("do_not_reply@hwaidakhotels.com", "ahmed.taha@titegypt.com", "Newsletter Request", body);
                return Ok(request);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return Ok(ex.Message);
            }
        }


        [HttpPost("MeetingEvents-Submit")]
        public async Task<ActionResult<TblRequestsContact>> CreateMeetingEventRequet(MeetingEventRequest request, [FromQuery] string hotelUrl)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(y => y.Errors).Select(e => e.ErrorMessage).ToList();

                return BadRequest(errors);
            }

            var hotel = await _context.Hotels.Where(x => x.HotelUrl == hotelUrl && x.HotelStatus == true).FirstOrDefaultAsync();
            var entity = new TblRequestsMeeting
            {
                CompanyName = request.CompanyName,
                EmailAddress = request.EmailAddress,
                EventEndDate = request.EventEndDate,
                EventStartDate = request.EventStartDate,
                FirstName = request.FirstName,
                HotelId = hotel.HotelId,
                JobTitle = request.JobTitle,
                LastName = request.LastName,
                Numberofattendees = request.Numberofattendees,
                Numberofguestroomsrequired = request.Numberofguestroomsrequired,
                PreferredSetup = request.PreferredSetup,
                RequestDate = DateTime.Now,
                SpecialRequest = request.SpecialRequest,
                TelephoneNumber = request.TelephoneNumber,
                IsArchive = false
            };


            var item = await _opContext.TblRequestsMeetings.AddAsync(entity);
            await _opContext.SaveChangesAsync();
            if (item == null)
                return BadRequest("ContactUs not save to DB");




            #region Send Mail
            string body =
                @"<table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                       <tbody>";



            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Name: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.FirstName + " " + entity.LastName + @" </td>
                       </tr>";




            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Email: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.EmailAddress + @" </td>
                       </tr>";

            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Mobile: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.TelephoneNumber + @" </td>
                       </tr>";

            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Hotel: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + hotel.HotelNameSys + @" </td>
                       </tr>";
            body +=
           @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Event Start Date: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.EventStartDate + @" </td>
                       </tr>";
            body +=
           @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Event End Date: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.EventEndDate + @" </td>
                       </tr>";
            body +=
           @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Job Title: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.JobTitle + @" </td>
                       </tr>";
            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Number Of Attendance: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.Numberofattendees + @" </td>
                       </tr>";
            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Number of guest room required: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.Numberofguestroomsrequired + @" </td>
                       </tr>";
            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Preferred Setup: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.PreferredSetup + @" </td>
                       </tr>";
            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Special Requets: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + entity.SpecialRequest + @" </td>
                       </tr>";
            body += @"</tbody></table> ";
            #endregion



            try
            {
                //_email.SendMail("do_not_reply@hwaidakhotels.com", entity.EmailAddress, "Meeting & Events Request", body);
                //_email.SendMail("do_not_reply@hwaidakhotels.com", "ahmed.taha@titegypt.com", "Meeting & Events Request", body);
                return Ok(entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                return Ok(ex.Message);
            }
        }

    }
}
