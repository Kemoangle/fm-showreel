using BackEnd.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Service;
using Showreel.Service.impl;

namespace Showreel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly ILanlordAdsService _lanlordAdsService;
        private readonly IRestrictionService _restrictionService;
        private readonly IVideoCategoryService _categoryService;
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService, ILanlordAdsService lanlordAdsService, IVideoService videoService, IRestrictionService restrictionService, IVideoCategoryService categoryService)
        {
            _buildingService = buildingService;
            _lanlordAdsService = lanlordAdsService;
            _videoService = videoService;
            _videoService = videoService;
            _restrictionService = restrictionService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult<Building> GetPageBuilding(string? keySearch = null, int page = 1, int pageSize = 10)
        {
            var query = _buildingService.GetAllBuildings(keySearch);
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
                Buildings = paginatedQuery.ToList()
            };

            return Ok(response);
        }
        class buildingLR
        {
            public int? id { get; set; }
            public Object? lanlordAds { get; set; }
            public Object? restriction { get; set; }
        };

        [HttpGet("detail")]
        public ActionResult<Building> GetAllBuildings(string listId)
        {
            string[] ids = listId.Split(",");

            List<buildingLR> listBuildings = new List<buildingLR>();
            foreach (var id in ids)
            {
                var lanlordAds = from l in _lanlordAdsService.GetLandlordAdsBuilding(Int32.Parse(id))
                                 select new
                                 {
                                     id = l.Id,
                                     loop = l.Loop,
                                     startDate = l.StartDate,
                                     endDate = l.EndDate,
                                     video = _videoService.GetVideoById((int)l.VideoId)
                                 };
                var restriction = from br in _restrictionService.GetBuildingRestriction(Int32.Parse(id))
                                  select new
                                  {
                                      id = br.Id,
                                      buildingId = br.BuildingId,
                                      type = br.Type,
                                      category = _categoryService.GetCategoryById((int)br.CategoryId),
                                      arrCategory = _restrictionService.GetRestrictionExcepts(br.Id)
                                  };

                listBuildings.Add(new buildingLR
                {
                    id = Int32.Parse(id),
                    lanlordAds = lanlordAds,
                    restriction = restriction
                });

            }
            return Ok(listBuildings);

        }

        [HttpGet("getAll")]
        public ActionResult<Building> GetAllBuildings()
        {
            var query = _buildingService.GetAllBuildings(null);
            return Ok(query);
        }

        [HttpGet("{id}")]
        public IActionResult GetBuildingById(int id)
        {
            var building = _buildingService.GetBuildingById(id);
            if (building == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }
            return Ok(building);
        }

        [HttpPost]
        public IActionResult CreateBuilding([FromBody] Building building)
        {
            var buildingExist = _buildingService.GetAllBuildings().ToList();
            if (buildingExist.Any(b => b.BuildingName?.ToUpper() == building.BuildingName?.ToUpper()))
            {
                ModelState.AddModelError("Building", "The name already exists");
                return BadRequest(ModelState);
            }
            building.CreateTime = DateTime.Now;
            building.LastUpdateTime = null;
            building.Landlordads = new List<Landlordad>();
            _buildingService.AddBuilding(building);
            return Ok("success");
        }


        [HttpPatch("{id}")]
        public IActionResult UpdateBuilding(int id, [FromBody] Building building)
        {
            var existingBuilding = _buildingService.GetBuildingById(id);
            if (existingBuilding == null)
            {
                return NotFound("Building not found");
            }
            var buildingName = _buildingService.GetAllBuildings().Where(b => b.Id != id).Select(b => b.BuildingName).ToList();
            if (buildingName.Contains(building.BuildingName))
            {
                ModelState.AddModelError("Building", "The name already exists");
                return BadRequest(ModelState);
            }
            building.LastUpdateTime = DateTime.Now;
            building.Landlordads = new List<Landlordad>();
            _buildingService.UpdateBuilding(building);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBuilding(int id)
        {
            _buildingService.DeleteBuilding(id);
            return Ok();
        }

        [HttpGet("getBuilding")]
        public ActionResult<Building> GetBuildings()
        {
            var query = _buildingService.GetBuildings();
            return Ok(query);
        }
    }
}
