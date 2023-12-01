using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Service;
using Showreel.Service.impl;

namespace Showreel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IVideoCategoryService _categoryService;
        public VideoController(IVideoService videoService, IVideoCategoryService categoryService)
        {
            _videoService = videoService;
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAllVideos()
        {
            var videos = _videoService.GetAllVideos();
            return Ok(videos);
        }

        [HttpGet]
        public IActionResult GetPageVideos(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            var videos = _videoService.GetPageVideos(keySearch);
            var query = from v in videos
                        select new
                        {
                            v.Id,
                            v.Title,
                            v.Duration,
                            v.KeyNo,
                            v.Rule,
                            category = _categoryService.GetCategoryByVideoId(v.Id)
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
                Videos = paginatedQuery.ToList()
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetVideoById(int id)
        {
            var video = _videoService.GetVideoById(id);
            if (video == null)
            {
                return NotFound();
            }
            var categories = _categoryService.GetCategoryByVideoId(id);
            var response = new
            {
                video.Id,
                video.KeyNo,
                video.Duration,
                video.CreateTime,
                video.LastUpdateTime,
                video.Title,
                video.Rule,
                category = categories.ToList()
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult CreateVideo([FromBody] Video video)
        {
            var videoExist = _videoService.GetAllVideos().ToList();
            if (videoExist.Any(b => b.KeyNo?.ToUpper() == video.KeyNo?.ToUpper()))
            {
                ModelState.AddModelError("Video", "The video already exists");
                return BadRequest(ModelState);
            }
            video.CreateTime = DateOnly.FromDateTime(DateTime.Now);
            video.LastUpdateTime = null;
            video.Landlordads = new List<Landlordad>();
            video.Videocategories = new List<Videocategory>();

            _videoService.AddVideo(video);

            return Ok(_videoService.GetVideoByKeyNo(video.KeyNo));
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] Video video)
        {
            var existingVideo = _videoService.GetVideoById(id);
            if (existingVideo == null)
            {
                return NotFound("Video not found");
            }
            var videoKeyNo = _videoService.GetAllVideos().Where(b => b.Id != id).Select(b => b.KeyNo).ToList();
            if (videoKeyNo.Contains(video.KeyNo))
            {
                ModelState.AddModelError("Video", "The video already exists");
                return BadRequest(ModelState);
            }

            video.LastUpdateTime = DateOnly.FromDateTime(DateTime.Now);
            video.Landlordads = new List<Landlordad>();
            video.Videocategories = new List<Videocategory>();
            _videoService.UpdateVideo(video);

            return Ok(_videoService.GetVideoByKeyNo(video.KeyNo));
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            _videoService.DeleteVideo(id);
            return Ok();
        }
    }
}
