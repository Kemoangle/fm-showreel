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
        private readonly IVideoService videoService;
        public VideoListController(IVideoListService _videoListService, IVideoService _videoService)
        {
            videoListService = _videoListService;
            videoService = _videoService;
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

        [HttpPost]
        public IActionResult Create([FromBody] Videolist videoList)
        {
            videoList.CreatedTime = DateOnly.FromDateTime(DateTime.Now);
            videoList.LastUpdatedTime = DateOnly.FromDateTime(DateTime.Now);
            var item = videoListService.AddVideoList(videoList);

            return Ok(item);
        }

        [HttpPatch("{id}")]
        public IActionResult CreateVideoList(int id, [FromBody] VideoVideolist[] videoVideolist)
        {
            foreach (var video in videoVideolist)
            {
                var newVideo = new VideoVideolist
                {
                    VideoId = video.VideoId,
                    LoopNum = video.LoopNum,
                    VideoListId = id
                };
                videoListService.AddVideoVideoList(newVideo);
            }
            
            return Ok();
        }


        [HttpGet("{id}")]
        public IActionResult GetAllVideoInList(int id)
        {
            var videoList = videoListService.GetVideoVideolist(id);
            var query = (from v in videoList 
                        select new{
                            video = videoService.GetVideoById((int)v.VideoId),
                            loopNum = v.LoopNum
                        }).ToList(); 
            return Ok(query);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            videoListService.DeleteVideoList(id);
            return Ok();
        }
    }
}