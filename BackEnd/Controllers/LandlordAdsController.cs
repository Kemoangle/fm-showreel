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
    public class LandlordAdsController : Controller
    {
        private readonly ILanlordAdsService lanlordAdsService;
        private readonly IVideoService videoService;

        public LandlordAdsController(ILanlordAdsService _lanlordAdsService, IVideoService _videoService)
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

        [HttpPost]
        public IActionResult CreateLandlordAds([FromBody] Landlordad landlordad)
        {
            var landlordadExist = lanlordAdsService.GetAllLandlordAds((int)landlordad.BuildingId);
            if (landlordadExist.Any(l => l.VideoId == landlordad.VideoId))
            {
                ModelState.AddModelError("Landlordad", "The video already exists");
                return BadRequest(ModelState);
            }
            lanlordAdsService.AddLandlordAds(landlordad);
            return Ok("success");
        }
        

        [HttpDelete("{id}")]
        public IActionResult DeleteLandlordAds(int id)
        {
            lanlordAdsService.DeleteLandlordAds(id);
            return Ok();
        }
    }
}