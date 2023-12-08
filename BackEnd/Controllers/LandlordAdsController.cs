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
                        select new
                        {
                            id = l.Id,
                            loop = l.Loop,
                            startDate = l.StartDate,
                            endDate = l.EndDate,
                            video = videoService.GetVideoById((int)l.VideoId)
                        };
            return Ok(query.ToList());
        }

        [HttpGet("building/{id}")]
        public IActionResult GetLandlordAdsBuilding(int id)
        {
            var query = from l in lanlordAdsService.GetAllLandlordAds(id)
                        select new
                        {
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

        [HttpGet("GetLandlordAdsById/{id}")]
        public IActionResult GetLandlordAdsById(int id)
        {
            var landlordads = lanlordAdsService.GetLandlordad(id);
            if (landlordads == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }
            var response = new
            {
                id = landlordads.Id,
                loop = landlordads.Loop,
                videoId = landlordads.VideoId,
                buildingId = landlordads.BuildingId,
                startDate = landlordads.StartDate,
                endDate = landlordads.EndDate,
            };
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateLandlordads(int id, [FromBody] Landlordad landlordads)
        {
            var videoExists = lanlordAdsService.GetAllLandlordAds((int)landlordads.BuildingId)
                                                .Where(l => l.Id != id)
                                                .Select(l => l.VideoId).ToList();
            if (videoExists.Contains(landlordads.VideoId))
            {
                ModelState.AddModelError("Landlordad", "The video already exists");
                return BadRequest(ModelState);
            }
            lanlordAdsService.UpdateLandlordAds(landlordads);
            return Ok();
        }
    }
}