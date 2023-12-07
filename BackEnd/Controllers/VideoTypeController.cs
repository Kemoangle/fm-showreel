using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Showreel.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoTypeController : Controller
    {
        private readonly IVideoTypeService videoTypeService;

        public VideoTypeController(IVideoTypeService _videoTypeService)
        {
            videoTypeService = _videoTypeService;
        }

        [HttpGet]
        public IActionResult GetAllVideotype()
        {
            return Ok(videoTypeService.GetAllVideoType().ToList());
        }

        [HttpGet("GetPageVideotype")]
        public IActionResult GetPageVideotype(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            var videoTypes = videoTypeService.GetPageVideoType(keySearch);
            var query = from c in videoTypes
                        select new
                        {
                            c.Id,
                            c.Name,
                        };

            int startIndex = (page - 1) * pageSize;
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paginatedQuery = query.Skip(startIndex).Take(pageSize);
            if (!paginatedQuery.ToList().Any())
            {
                page = 1;
                totalPages = 1;
            }
            var response = new
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = page,
                PageSize = pageSize,
                videoTypes = paginatedQuery.ToList()
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetVideoTypeById(int id)
        {
            var videoType = videoTypeService.GetVideoTypeById(id);
            if (videoType == null)
            {
                return NotFound();
            }
            return Ok(videoType);
        }

        [HttpPost]
        public IActionResult CreateVideoType([FromBody] VideoType videoType)
        {
            var videoTypeExist = videoTypeService.GetAllVideoType().ToList();
            if (videoTypeExist.Any(b => b.Name?.ToUpper() == videoType.Name?.ToUpper()))
            {
                ModelState.AddModelError("VideoType", "The video type already exists");
                return BadRequest(ModelState);
            }
            videoTypeService.AddVideoType(videoType);
            return Ok();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateVideoType(int id, [FromBody] VideoType videoType)
        {
            var videoTypeExist = videoTypeService.GetVideoTypeById(id);
            if (videoTypeExist == null)
            {
                return NotFound("Video not found");
            }
            var videoTypeName = videoTypeService.GetAllVideoType().Where(b => b.Id != id).Select(b => b.Name).ToList();
            if (videoTypeName.Contains(videoType.Name))
            {
                ModelState.AddModelError("VideoType", "The video type already exists");
                return BadRequest(ModelState);
            }
            videoTypeService.UpdateVideoType(videoType);
            return Ok();
        }

        [HttpGet("GetVideoTypeByCategory/{id}")]
        public IActionResult GetVideoTypeByCategory(int id)
        {
            var videoType = videoTypeService.GetVideoTypeByCategory(id);
            return Ok(videoType);
        }
    }
}