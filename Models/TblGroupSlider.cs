using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class TblGroupSlider
{
    public int SliderGroupId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? LangId { get; set; }

    public bool? SliderStatus { get; set; }

    public int? SliderPosition { get; set; }

    public string SliderPhoto { get; set; }

    public string SliderMainText { get; set; }

    public string SliderSubText { get; set; }

    public string SliderButtonText { get; set; }

    public string SliderbuttonUrl { get; set; }

    public bool? IsArchive { get; set; }

    public bool? IsDisplayContent { get; set; }

    public DateTime? DeletedDate { get; set; }

    public double? SliderPhotoWidth { get; set; }

    public double? SliderPhotoHieght { get; set; }

    public string SliderPhotoTablet { get; set; }

    public int? SliderPhotoTabletWidth { get; set; }

    public int? SliderPhotoTabletHeight { get; set; }

    public string SliderPhotoMobile { get; set; }

    public int? SliderPhotoMobileWidth { get; set; }

    public int? SliderPhotoMobileHeight { get; set; }

    public virtual MasterLanguage Lang { get; set; }
}
