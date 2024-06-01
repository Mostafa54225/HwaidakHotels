using AutoMapper;
using HwaidakAPI.DTOs.Responses.Restaurants;
using HwaidakAPI.DTOs;
using HwaidakAPI.Errors;
using HwaidakAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HwaidakAPI.DTOs.Responses.ContactUs;
using System;
using HwaidakAPI.OPModels;
using OrientHGAPI.DTOs.Responses.ContactUs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrientHGAPI.Helpers;
using HwaidakAPI.DTOs.Requests;
using HwaidakAPI.DTOs.Responses.Hotels;

namespace HwaidakAPI.Controllers
{
    public class HealthCheckController : BaseApiController
    {
        [HttpGet("~/health")]
        public ActionResult HealthCheck()
        {
            return Ok();
        }
    }
}
