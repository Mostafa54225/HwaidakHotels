using System;
using System.Collections.Generic;

namespace HwaidakAPI.Models;

public partial class TblTestimonial
{
    public int TestimonialId { get; set; }

    public DateTime? TestimonialDateTime { get; set; }

    public string TestimonialTitle { get; set; }

    public string TestimonialDetails { get; set; }

    public string TestimonialCountry { get; set; }

    public bool? TestimonialStatus { get; set; }

    public int? TestimonialPosition { get; set; }

    public int? LangId { get; set; }

    public string TestimonialCustomerName { get; set; }

    public string TestimonialCustomerEmail { get; set; }

    public int? HotelId { get; set; }
}
