using System;
using System.Collections.Generic;

namespace HwaidakAPI.CheckInModels;

public partial class RequestsGuest
{
    public int GuestId { get; set; }

    public int? RequestId { get; set; }

    public string GuestName { get; set; }

    public string GuestBirthDate { get; set; }

    public string GuestPassport { get; set; }

    public string GuestUploadFile { get; set; }

    public virtual Request Request { get; set; }
}
