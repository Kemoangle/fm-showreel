using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Service;
using Showreel.Service.impl;

namespace Showreel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IVideoCategoryService _categoryService;
        public CategoryController(IVideoService videoService, IVideoCategoryService categoryService)
        {
            _videoService = videoService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAllCategoriess()
        {
            var response = _categoryService.GetAllCategory();

            return Ok(response);
        }

        [HttpGet("GetCategoryByVideo/{id}")]
        public IActionResult GetCategoryByVideoId(int id)
        {
            var categories = _categoryService.GetCategoryByVideoId(id);
            return Ok(categories);
        }


        [HttpPatch("{id}")]
        public IActionResult UpdateVideoCategory(int id, [FromBody] Category[] categories)
        {
            _categoryService.UpdateVideoCategory(categories, id);
            return Ok("success");
        }
    }
}
