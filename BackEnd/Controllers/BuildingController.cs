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
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
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

        [HttpGet("getAll")]
        public ActionResult<Building> GetAllBuildings(bool isGetLandlord = false)
        {
            var query = _buildingService.GetAllBuildings(null, isGetLandlord);
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
            building.CreateTime = DateOnly.FromDateTime(DateTime.Now);
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
            building.LastUpdateTime = DateOnly.FromDateTime(DateTime.Now);
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
    }
}
