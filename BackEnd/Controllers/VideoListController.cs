using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Service;

namespace Showreel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoListController : ControllerBase
    {
        private readonly IVideoListService videoListService;
        public VideoListController(IVideoListService _videoListService)
        {
            videoListService = _videoListService;
        }
        
        [HttpGet("getAll")]
        public ActionResult<Videolist> GetAllVideoList()
        {
            var query = videoListService.GetAllVideoList();
            return Ok(query);
        }

        [HttpGet]
        public ActionResult<Videolist> GetPageVideolist(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            var query = videoListService.GetPageVideoList(keySearch);
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
                Videolist = paginatedQuery.ToList()
            };

            return Ok(response);
        }
    }
}