using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Showreel.Models;
using Showreel.Service;

namespace Showreel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanlordAdsController : Controller
    {
        private readonly ILanlordAdsService lanlordAdsService;
        private readonly IVideoService videoService;

        public LanlordAdsController(ILanlordAdsService _lanlordAdsService, IVideoService _videoService)
        {
            lanlordAdsService = _lanlordAdsService;
            videoService = _videoService;
        }

        [HttpGet("{id}")]
        public IActionResult GetAllLandlordAds(int id)
        {
            var query = from l in lanlordAdsService.GetAllLandlordAds(id)
                        select new {
                            id = l.Id,
                            loop = l.Loop,
                            startDate = l.StartDate,
                            endDate = l.EndDate,
                            video = videoService.GetVideoById((int)l.VideoId)
                        };
            return Ok(query.ToList());
        }

    }
}