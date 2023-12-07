using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;

namespace Showreel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestrictionController : ControllerBase
    {
        private readonly IRestrictionService restrictionService;
        public RestrictionController(IRestrictionService _restrictionService)
        {
            restrictionService = _restrictionService;
        }

        [HttpGet("{id}")]
        public IActionResult GetAllRestriction(int id)
        {
            var query = from br in restrictionService.GetBuildingRestriction(id)
                        select new
                        {
                            id = br.Id,
                            buildingId = br.BuildingId,
                            type = br.Type,
                            name = br.Name,
                            except = restrictionService.GetVideoExcept(br.Id)
                        };
            return Ok(query.ToList());
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateRestrictionExcept(int id, [FromBody] VideoType[] videoTypes)
        {
            restrictionService.UpdateRestrictionExcept(videoTypes, id);
            return Ok("success");
        }

        [HttpPost]
        public IActionResult AddBuildingRestriction([FromBody] BuildingRestriction buildingRestriction)
        {
            var restrictionExist = restrictionService.GetBuildingRestriction((int)buildingRestriction.BuildingId).ToList();
            if (restrictionExist.Any(b => b.CategoryId == buildingRestriction.CategoryId))
            {
                ModelState.AddModelError("Restriction", "The restriction already exists");
                return BadRequest(ModelState);
            }
            var response = restrictionService.AddBuildingRestriction(buildingRestriction);
            return Ok(response);
        }
    }
}