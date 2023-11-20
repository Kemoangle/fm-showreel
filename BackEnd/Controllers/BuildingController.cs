using Microsoft.AspNetCore.Http;
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
        public IActionResult GetAllBuildings()
        {
            //var buildings = _buildingService.GetAllBuilding();
            //return Ok(buildings);
            return Ok("kllk");
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
            if (buildingExist.Any(b => b.BuildingName.ToUpper() == building.BuildingName.ToUpper()))
            {
                ModelState.AddModelError("Name", "The name already exists");
                return BadRequest(ModelState);
            }
            building.CreateTime = DateOnly.FromDateTime(DateTime.Now);
            building.LastUpdateTime = null;
            building.Landlordads = new List<Landlordad>();
            _buildingService.AddBuilding(building);
            return Ok("success");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBuilding(int id, [FromBody] Building building)
        {
            var existingBuilding = _buildingService.GetBuildingById(id);
            if (existingBuilding == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }

            existingBuilding.BuildingName = building.BuildingName;
            _buildingService.UpdateBuilding(existingBuilding);

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
