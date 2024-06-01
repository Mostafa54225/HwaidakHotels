using HwaidakAPI.CheckInModels;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HwaidakAPI.DTOs.Requests
{
    public class CheckInOnlineRequestDto
    {


        //Booking Details
        public string HotelName { get; set; }
        public string GuestName { get; set; }
        public string ReservationThrough { get; set; }
        public string ChannelName { get; set; }
        public string CheckInDate { get; set; }
        public string CheckoutDate { get; set; }
        public string ArrivalFlight { get; set; }
        public string DepartureFlight { get; set; }

        public string NumberofRooms { get; set; }

        //Personal Details
        public string Nationality { get; set; }
        public string GuestBirthDate { get; set; }
        public string Passport { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }






        //Medical
        public bool chronicdiseases { get; set; }
        public string chronicdiseasesdescription { get; set; }
        public bool last14days { get; set; }
        public string last14daysdescription { get; set; }





        //Documents / Papers
        public IFormFile ScanFile { get; set; }
        public IFormFile ScanFileWife { get; set; }
        public IFormFile MarriageCertificate { get; set; }
        public IFormFile DepositReceipt { get; set; }



        public string SpecialRequest { get; set; }
        //Companion's Details
        public List<CompanionsRequest> CompanionsGuests { get; set; }
    }
}
