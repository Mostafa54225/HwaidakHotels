using System;
using System.Collections.Generic;

namespace HwaidakAPI.CheckInModels;

public partial class Request
{
    public int RequestId { get; set; }

    public DateTime? RequestDate { get; set; }

    public string HotelName { get; set; }

    public string GuestName { get; set; }

    public string GuestBirthDate { get; set; }

    public string Nationality { get; set; }

    public string Passport { get; set; }

    public string MobileNumber { get; set; }

    public string EmailAddress { get; set; }

    public string CheckInDate { get; set; }

    public string CheckoutDate { get; set; }

    public string ArrivalFlight { get; set; }

    public string DepartureFlight { get; set; }

    public string ReservationThrough { get; set; }

    public string ChannelName { get; set; }

    public string NumberofRooms { get; set; }

    public string RoomTypes { get; set; }

    public string ScanFile { get; set; }

    public string ScanFileWife { get; set; }

    public string MarriageCertificate { get; set; }

    public string DepositReceipt { get; set; }

    public bool? Chronicdiseases { get; set; }

    public string Chronicdiseasesdescription { get; set; }

    public bool? Last14days { get; set; }

    public string Last14daysdescription { get; set; }

    public string SpecialRequest { get; set; }

    public string NumberofGuest { get; set; }

    public virtual ICollection<RequestsGuest> RequestsGuests { get; set; } = new List<RequestsGuest>();
}
