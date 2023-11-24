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

        [HttpGet]
        public IActionResult GetAllVideos()
        {
            var videos = _videoService.GetAllVideos();
            var videocategory = _categoryService.GetAllCategory();
            var query = from v in videos
                        select new {
                            videos = v,
                            category = _categoryService.GetCategoryByVideoId(v.Id)
                        };

            return Ok(query.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetVideoById(int id)
        {
            var building = _videoService.GetVideoById(id);
            if (building == null)
            {
                return NotFound();
            }
            return Ok(building);
        }

        [HttpPost]
        public IActionResult CreateVideo([FromBody] Video video)
        {
            var videoExist = _videoService.GetAllVideos().ToList();
            if (videoExist.Any(b => b.KeyNo.ToUpper() == video.KeyNo.ToUpper()))
            {
                ModelState.AddModelError("Name", "The name already exists");
                return BadRequest(ModelState);
            }
            _videoService.AddVideo(video);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateVideo(int id, [FromBody] Video video)
        {
            var existingVideo = _videoService.GetVideoById(id);
            if (existingVideo == null)
            {
                return NotFound();
            }

            existingVideo.Title = video.Title; // Update other properties as needed
            _videoService.UpdateVideo(existingVideo);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVideo(int id)
        {
            _videoService.DeleteVideo(id);
            return Ok();
        }
    }
}
