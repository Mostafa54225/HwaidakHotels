using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class TblExcursion
{
    public int ExcursionId { get; set; }

    public string ExcursionNameSys { get; set; }

    public string ExcursionPhoto { get; set; }

    public bool? ExcursionStatus { get; set; }

    public int? ExcursionPosition { get; set; }

    public int? ExcursionPhotoWidth { get; set; }

    public int? ExcursionPhotoHeight { get; set; }

    public bool? IsDetails { get; set; }

    public string Banner { get; set; }

    public int? BannerWidth { get; set; }

    public int? BannerHeight { get; set; }

    public string BannerTablet { get; set; }

    public int? BannerTabletWidth { get; set; }

    public int? BannerTabletHeight { get; set; }

    public string BannerMobile { get; set; }

    public int? BannerMobileWidth { get; set; }

    public int? BannerMobileHeight { get; set; }

    public virtual ICollection<TblExcursionContent> TblExcursionContents { get; set; } = new List<TblExcursionContent>();
}
