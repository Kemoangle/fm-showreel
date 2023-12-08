using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Service;
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
        private readonly IVideoCategoryService categoryService;
        private readonly IRuleService ruleService;

        public VideoListController(
            IVideoListService _videoListService, 
            IVideoService _videoService, 
            IVideoCategoryService _categoryService,
            IRuleService _ruleService
            )
        {
            videoListService = _videoListService;
            videoService = _videoService;
            categoryService = _categoryService;
            ruleService = _ruleService;
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
            var videoListExist = videoListService.GetAllVideoList().ToList();
            if (videoListExist.Any(b => b.Title?.ToUpper() == videoList.Title?.ToUpper()))
            {
                ModelState.AddModelError("VideoList", "The video list already exists");
                return BadRequest(ModelState);
            }
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
                         select new
                         {
                             video = videoService.GetVideoById((int)v.VideoId),
                             category = categoryService.GetCategoryByVideoId((int)v.VideoId),
                             loopNum = v.LoopNum,
                             doNotPlay = ruleService.GetDoNotPlay((int)v.VideoId),
                             noBackToBack = ruleService.GetNoBackToback((int)v.VideoId)
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