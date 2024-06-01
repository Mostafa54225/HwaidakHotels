using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class TblExcursionContent
{
    public int ExcursionContentId { get; set; }

    public int? ExcursionId { get; set; }

    public int? LangId { get; set; }

    public string ExcursionName { get; set; }

    public string ExcursionContent { get; set; }

    public string ExcursionIntro { get; set; }

    public string ExcursionMetatagTitle { get; set; }

    public string ExcursionMetatagDescription { get; set; }

    public virtual TblExcursion Excursion { get; set; }

    public virtual MasterLanguage Lang { get; set; }
}
