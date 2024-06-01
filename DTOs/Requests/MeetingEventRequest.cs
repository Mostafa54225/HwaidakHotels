namespace HwaidakAPI.DTOs.Requests
{
    public class MeetingEventRequest
    {
        public string Numberofattendees { get; set; }
        public string Numberofguestroomsrequired { get; set; }
        public string PreferredSetup { get; set; }
        public DateTime? EventStartDate { get; set; }
        public DateTime? EventEndDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string TelephoneNumber { get; set; }
        public string SpecialRequest { get; set; }
    }
}
