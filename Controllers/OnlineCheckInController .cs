using AutoMapper;
using HwaidakAPI.CheckInModels;
using HwaidakAPI.DTOs;
using HwaidakAPI.DTOs.Requests;
using HwaidakAPI.DTOs.Responses.CheckInOnline;
using HwaidakAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrientHGAPI.Helpers;

namespace HwaidakAPI.Controllers
{
    public class OnlineCheckInController : BaseApiController
    {
        private readonly HwaidakHotelsOnlineCheckInDbContext _onliceCheckInContext;
        private readonly HwaidakHotelsWsdbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _email;
        private readonly IWebHostEnvironment _env;

        public OnlineCheckInController(HwaidakHotelsWsdbContext context, HwaidakHotelsOnlineCheckInDbContext onliceCheckInContext, IMapper mapper, IWebHostEnvironment env, IConfiguration configuration, IEmailSender email)
        {
            _onliceCheckInContext = onliceCheckInContext;
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _email = email;
            _env = env;
        }

        [HttpGet("{languageCode}")]
        public async Task<ActionResult<CheckOnlineResponseDto>> GetOnlineCheckIn(string languageCode = "en")
        {
            var groupPage = await _context.VwGroupPages.Where(x => x.LanguageAbbreviation == languageCode).FirstOrDefaultAsync();


            var checkinhotels = await _onliceCheckInContext.CheckInHotels.Where(x => x.HotelStatus == true).OrderBy(x => x.HotelPosition).ToListAsync();
            var reservationThrough = await _onliceCheckInContext.CheckInReservationThroughs.ToListAsync();
            MainResponse pagedetails = new MainResponse
            {
                PageTitle = groupPage.GroupCheckInTitle,
                PageBannerPC = _configuration["ImagesLink"] + groupPage.GroupCheckInBanner,
                PageBannerMobile = _configuration["ImagesLink"] + groupPage.GroupCheckInBannerMobile,
                PageBannerTablet = _configuration["ImagesLink"] + groupPage.GroupCheckInBannerTablet,
                PageText = groupPage.GroupCheckIn,
                PageMetatagTitle = groupPage.GroupCheckInMetatagTitle,
                PageMetatagDescription = groupPage.GroupCheckInMetatagDescription
            };

            var hotels = _mapper.Map<List<CheckInHotels>>(checkinhotels);
            var reservationsthrough = _mapper.Map<List<CheckInReservationth>>(reservationThrough);

            CheckOnlineResponseDto model = new CheckOnlineResponseDto()
            {
                PageDetails = pagedetails,
                CheckInHotels = hotels,
                CheckInReservationThroughs = reservationsthrough
            };

            return Ok(model);

        }


        [HttpPost]
        public async Task<ActionResult> PostOnlineCheckIn([FromForm]CheckInOnlineRequestDto request)
        {
            var req = new Request
            {
                ArrivalFlight = request.ArrivalFlight,
                ChannelName = request.ChannelName,
                CheckInDate = request.CheckInDate,
                CheckoutDate = request.CheckoutDate,
                Chronicdiseases = request.chronicdiseases,
                Chronicdiseasesdescription = request.chronicdiseasesdescription,
                DepartureFlight = request.DepartureFlight,
                //DepositReceipt = request.DepositReceipt,
                EmailAddress = request.EmailAddress,
                GuestBirthDate = request.GuestBirthDate,
                GuestName = request.GuestName,
                HotelName = request.HotelName,
                Last14days = request.last14days,
                Last14daysdescription = request.last14daysdescription,
                //MarriageCertificate = request.MarriageCertificate,
                MobileNumber = request.MobileNumber,
                Nationality = request.Nationality,
                NumberofRooms = request.NumberofRooms,
                Passport = request.Passport,
                RequestDate = DateTime.Now,
                RequestsGuests = new List<RequestsGuest>(), // Ensure this is initialized as a new list
                ReservationThrough = request.ReservationThrough,
                NumberofGuest = request.CompanionsGuests.Count.ToString(),
                //ScanFile = request.ScanFile,
                //ScanFileWife = request.ScanFileWife,
                SpecialRequest = request.SpecialRequest
            };
            var uploads = Path.Combine(_env.WebRootPath, "uploadfiles");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            string marriageFileName = generateFileName(request.MarriageCertificate.FileName);
            var MarriageCertificatefilePath = Path.Combine(uploads, marriageFileName);
            using (var fileStream = new FileStream(MarriageCertificatefilePath, FileMode.Create))
            {
                await request.MarriageCertificate.CopyToAsync(fileStream);
            }

            string scanFileName = generateFileName(request.ScanFile.FileName);
            var ScanFilefilePath = Path.Combine(uploads, scanFileName);
            using (var fileStream = new FileStream(ScanFilefilePath, FileMode.Create))
            {
                await request.ScanFile.CopyToAsync(fileStream);
            }
            string scanfilewifename = generateFileName(request.ScanFileWife.FileName);
            var ScanFileWifefilePath = Path.Combine(uploads, scanfilewifename);
            using (var fileStream = new FileStream(ScanFileWifefilePath, FileMode.Create))
            {
                await request.ScanFileWife.CopyToAsync(fileStream);
            }

            string depositeFileName = generateFileName(request.DepositReceipt.FileName);
            var DepositReceiptfilePath = Path.Combine(uploads, depositeFileName);
            using (var fileStream = new FileStream(DepositReceiptfilePath, FileMode.Create))
            {
                await request.DepositReceipt.CopyToAsync(fileStream);
            }
            req.ScanFile = scanFileName;
            req.ScanFileWife = scanfilewifename;
            req.MarriageCertificate = marriageFileName;
            req.DepositReceipt = depositeFileName;


            // Add the new request to the context and save changes
            _onliceCheckInContext.Requests.Add(req);
            await _onliceCheckInContext.SaveChangesAsync();

            var requestId = req.RequestId;

            // Prepare the list of guests to add
            var guestsToAdd = new List<RequestsGuest>();

            if (request.CompanionsGuests != null)
            {
                foreach (var item in request.CompanionsGuests)
                {
                    string fileGuestName = generateFileName(item.GuestUploadFile.FileName);

                    var guest = new RequestsGuest
                    {
                        RequestId = requestId,
                        GuestBirthDate = item.GuestBirthDate,
                        GuestName = item.GuestName,
                        GuestPassport = item.GuestPassport,
                        GuestUploadFile = fileGuestName
                    };


                    var uploadsguest = Path.Combine(_env.WebRootPath, "uploadfiles");

                    if (!Directory.Exists(uploadsguest))
                    {
                        Directory.CreateDirectory(uploadsguest);
                    }
                    var filePathguest = Path.Combine(uploadsguest, fileGuestName);
                    using (var fileStream = new FileStream(filePathguest, FileMode.Create))
                    {
                        await item.GuestUploadFile.CopyToAsync(fileStream);
                    }
                    //guest.GuestUploadFile = item.GuestUploadFile.FileName;

                    guestsToAdd.Add(guest);
                }
            }

            // Add all the guests to the context in one go
            if (guestsToAdd.Any())
            {
                _onliceCheckInContext.RequestsGuests.AddRange(guestsToAdd);
                await _onliceCheckInContext.SaveChangesAsync();
            }


            



            #region mail


            // Personal Details
            string body =
            @"<h3>Personal Details</h3><table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                       <tbody>";


            body +=
               @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Guest Name: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.GuestName + @" </td>
                       </tr>";

            body +=
          @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Nationality: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.Nationality + @" </td>
                       </tr>";


            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Birthdate: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.GuestBirthDate + " " + "" + @" </td>
                       </tr>";

            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Passport: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.Passport + @" </td>
                       </tr>";

            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Mobile Number: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.MobileNumber + @" </td>
                       </tr>";



            body +=
         @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Email: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.EmailAddress + @" </td>
                       </tr>";

            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Number Of Rooms: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.NumberofRooms + @" </td>
                       </tr>";

            if (request.SpecialRequest != "" && request.SpecialRequest != null)
            {

                body +=
              @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Special Requirement: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.SpecialRequest + @" </td>
                       </tr>";
            }
            body += @"</tbody></table><br>";



            //
            //Guests
            if (guestsToAdd.Count > 0)
            {
                body += @"
           <br><h3>Companions Details</h3><br><h2>Number Of Rooms: " + request.NumberofRooms + @"
</h2> <table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                <thead style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                    <tr>
                        <th class='text-center'> Name </th>
                        <th class='text-center'> Date Of Birth  </th>
                        <th class='text-center'>Passport Number </th>
                        <th class='text-center'>File </th>
                    </tr>
                </thead>
                <tbody>";

                foreach (var item in guestsToAdd)
                {

                    body += @"<tr>
                <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + item.GuestName + @" </td>
                <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + DateTime.Parse(item.GuestBirthDate.ToString()).ToString("dd/MM/yyyy") + @" </td>
                <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + item.GuestPassport + @" </td>
                <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + item.GuestUploadFile + @" </td>
            </tr>";
                }

                body += @"</tbody></table><br>";
            }

           
            // Booking Details
            body += @"<h3>Booking Details</h3><table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                       <tbody>";

            body +=
            @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Hotel Name: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.HotelName + @" </td>
                       </tr>";

            body +=
          @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Reservation Through: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.ReservationThrough + @" </td>
                       </tr>";

            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Channel/Travel Agent: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.ChannelName + @" </td>
                       </tr>";

            body +=
          @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Check In Date: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.CheckInDate + @" </td>
                       </tr>";


            body +=
          @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Check out Date: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.CheckoutDate + @" </td>
                       </tr>";
            body +=
@"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Arrival Flight: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.ArrivalFlight + @" </td>
                       </tr>";

            body +=
         @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Departure Flight: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.DepartureFlight + @" </td>
                       </tr></tbody></table><br>";



            // Disease Details
            if(request.chronicdiseases == true || request.last14days == true)
            {
                body += @"<h3>Medical Information</h3><table  style='border:1px solid #ccc;text-align: left;border-collapse: collapse;width: 100%;'>
                       <tbody>";
                if (request.chronicdiseases == true)
                {
                    body +=
                    @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Chronic Disease Details: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.chronicdiseasesdescription + @" </td>
                       </tr>";
                }
                if (request.last14days == true)
                {
                    body +=
                    @"<tr style='border: 1px solid #ccc;text-align: left;padding: 15px;'>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;width:200px;background-color:#ddd;'>Last 14 Days Details: </td>
                       <td style='border: 1px solid #ccc;text-align: left;padding: 15px;'> " + request.last14daysdescription + @" </td>
                       </tr>";
                }
            }

            

            body += @"</tbody></table> ";

            
                //_email.SendMail("do_not_reply@hwaidakhotels.com", request.EmailAddress, "Online Check In Request", body);

            




            #endregion



            return Ok();

        }




        private string generateFileName(string fileName)
        {
            Random rd = new Random();
            int rand_num = rd.Next(100, 20000);
            #region Remove Special Character
            fileName = Path.GetFileName(fileName).Replace(" ", "_");
            fileName = fileName.Replace("&", "_");
            fileName = fileName.Replace("~", "_");
            fileName = fileName.Replace("`", "_");
            fileName = fileName.Replace("!", "_");
            fileName = fileName.Replace("@", "_");
            fileName = fileName.Replace("#", "_");
            fileName = fileName.Replace("$", "_");
            fileName = fileName.Replace("%", "_");
            fileName = fileName.Replace("^", "_");
            fileName = fileName.Replace("*", "_");
            fileName = fileName.Replace("(", "_");
            fileName = fileName.Replace(")", "_");
            fileName = fileName.Replace("–", "_");
            fileName = fileName.Replace("+", "_");
            fileName = fileName.Replace("=", "_");
            fileName = fileName.Replace("{", "_");
            fileName = fileName.Replace("[", "_");
            fileName = fileName.Replace("}", "_");
            fileName = fileName.Replace("]", "_");
            fileName = fileName.Replace("|", "_");
            fileName = fileName.Replace(":", "_");
            fileName = fileName.Replace(";", "_");
            fileName = fileName.Replace("“", "_");
            fileName = fileName.Replace("‘", "_");
            fileName = fileName.Replace("<", "_");
            fileName = fileName.Replace(",", "_");
            fileName = fileName.Replace(">", "_");
            fileName = fileName.Replace("?", "_");
            fileName = fileName.Replace("/", "_");
            fileName = fileName.Replace("__", "_");
            #endregion
            return rand_num + "_" + fileName;
        }

    }
}
