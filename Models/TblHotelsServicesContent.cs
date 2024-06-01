using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class TblHotelsServicesContent
{
    public int ServiceContentId { get; set; }

    public string SeviceName { get; set; }

    public int? LangId { get; set; }

    public int? ServiceId { get; set; }

    public virtual MasterLanguage Lang { get; set; }

    public virtual TblHotelsService Service { get; set; }
}
