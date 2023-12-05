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

        [HttpGet("GetPageCategory")]
        public IActionResult GetPageCategoriess(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            var categories = _categoryService.GetPageCategory(keySearch);
            var query = from c in categories
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
                categories = paginatedQuery.ToList()
            };

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

        [HttpPost]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            var categoryExist = _categoryService.GetAllCategory().ToList();
            if (categoryExist.Any(b => b.Name?.ToUpper() == category.Name?.ToUpper()))
            {
                ModelState.AddModelError("Category", "The Category already exists");
                return BadRequest(ModelState);
            }
            _categoryService.AddCategory(category);
            return Ok();
        }

        [HttpPatch("UpdateCategory/{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            var categoryName = _categoryService.GetAllCategory().Where(b => b.Id != id).Select(b => b.Name).ToList();
            if (categoryName.Contains(category.Name))
            {
                ModelState.AddModelError("Category", "The Category already exists");
                return BadRequest(ModelState);
            }
            _categoryService.UpdateCategory(category);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
    }
}
