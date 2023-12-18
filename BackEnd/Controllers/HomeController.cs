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
    [Route("/")]
    public class HomeController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IVideoCategoryService _categoryService;
        private readonly IRuleService _ruleService;
        public HomeController(
            IVideoService videoService,
            IVideoCategoryService categoryService,
            IRuleService ruleService
        )
        {
            _videoService = videoService;
            _categoryService = categoryService;
            _ruleService = ruleService;
        }
        [HttpGet]
        public IActionResult Get(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            return Ok("Showreel api!");
            // var videos = _videoService.GetPageVideos(keySearch);
            // var query = from v in videos
            //             select new
            //             {
            //                 v.Id,
            //                 v.Title,
            //                 v.Duration,
            //                 v.KeyNo,
            //                 v.Remark,
            //                 category = _categoryService.GetCategoryByVideoId(v.Id),
            //                 subCategory = _categoryService.GetSubCategoryByVideoId(v.Id),
            //                 doNotPlay = _ruleService.GetDoNotPlay(v.Id),
            //                 noBackToBack = _ruleService.GetNoBackToback(v.Id)
            //             };

            // int startIndex = (page - 1) * pageSize;
            // int totalItems = query.Count();
            // int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // var paginatedQuery = query.Skip(startIndex).Take(pageSize);
            // if (!paginatedQuery.ToList().Any())
            // {
            //     page = 1;
            //     totalPages = 1;
            // }
            // var response = new
            // {
            //     TotalItems = totalItems,
            //     TotalPages = totalPages,
            //     CurrentPage = page,
            //     PageSize = pageSize,
            //     Videos = paginatedQuery.ToList()
            // };

            // return Ok(response);
        }
    }
}