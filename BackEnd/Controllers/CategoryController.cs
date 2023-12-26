using System.Collections.Generic;
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

        [HttpGet("GetSub")]
        public IActionResult GetSubCategoriess([FromQuery] int[] categories)
        {
            var response = _categoryService.GetCategoryByParent(arrParentId: categories);
            
            return Ok(response);
        }

        [HttpGet("GetParent")]
        public IActionResult GetParentCategoriess()
        {
            var query = _categoryService.GetAllParentCategory();
            var response = (from c in query
                            select new {
                                c.Id,
                                c.Name,
                                c.Parent,
                                subCategory = _categoryService.GetCategoryByParent(parentId: c.Id)
                            }).ToList();                
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
                            subCategory = _categoryService.GetCategoryByParent(c.Id)
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
            var categoryExist = _categoryService.GetAllParentCategory().Select(c => c.Name).ToList();
            if (categoryExist.Contains(category.Name))
            {
                ModelState.AddModelError("Category", "The Category already exists");
                return BadRequest(ModelState);
            }
            category.Restrictions = new List<Restriction>();
            category.Rules = new List<Rule>();
            category.Videocategories = new List<Videocategory>();
            var response = _categoryService.AddCategory(category);
            return Ok(response);
        }

        [HttpPatch("UpdateCategory/{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            var categoryName = _categoryService.GetAllParentCategory().Where(b => b.Id != id).Select(b => b.Name).ToList();
            if (categoryName.Contains(category.Name))
            {
                ModelState.AddModelError("Category", "The Category already exists");
                return BadRequest(ModelState);
            }
            category.Restrictions = new List<Restriction>();
            category.Rules = new List<Rule>();
            category.Videocategories = new List<Videocategory>();
            var response = _categoryService.UpdateCategory(category);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            var response = new {
                id = category.Id,
                name = category.Name,
                subCategory = _categoryService.GetCategoryByParent(category.Id),
            };
            return Ok(response);
        }

        [HttpGet("GetCategoryByParent/{id}")]
        public IActionResult GetCategoryByParent(int id)
        {
            var categories = _categoryService.GetCategoryByParent(id);
            return Ok(categories);
        }

        [HttpPatch("UpdateSubCategory/{id}")]
        public IActionResult UpdateSubCategory([FromBody] Category[] categories, int id)
        {
            _categoryService.UpdateSubCategory(categories, id);
            return Ok();
        }

        [HttpGet("GetAllCategory")]
        public IActionResult GetAllCategory(string? keySearch = null)
        {
            var categories = _categoryService.GetPageCategory(keySearch);
            var query = from c in categories
                        select new
                        {
                            c.Id,
                            c.Name,
                            subCategory = _categoryService.GetCategoryByParent(c.Id)
                        };
            return Ok(query.ToList());
        }
    }
}
