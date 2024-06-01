
namespace HwaidakAPI.DTOs.Requests
{
    public class CompanionsRequest
    {
        public string GuestName { get; set; }
        public string GuestBirthDate { get; set; }
        public string GuestPassport { get; set; }
        public IFormFile GuestUploadFile { get; set; }
    }
}
