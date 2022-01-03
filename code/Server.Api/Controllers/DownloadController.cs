using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Dtos;
using Server.Api.Enum;
using Server.Api.Helpers;
using Server.Api.Models;
using Server.Api.Repositories;
using Server.Api.Services;

namespace Server.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        public DownloadController() { }

        [HttpGet("apk")]
        public ActionResult Apk()
        {
            string filePath = Directory.GetCurrentDirectory()+"/Downloads/Stunet.apk";
            string fileName = "Stunet.apk";

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, "application/force-download", fileName);

        }
    }
}
