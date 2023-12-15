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
    [Route("api/[controller]")]
    public class RestrictionController : ControllerBase
    {
        private readonly IRestrictionService restrictionService;
        private readonly IVideoCategoryService _categoryService;
        public RestrictionController(IRestrictionService _restrictionService, IVideoCategoryService categoryService)
        {
            restrictionService = _restrictionService;
            _categoryService = categoryService;
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
                            category = _categoryService.GetCategoryById((int)br.CategoryId),
                            arrCategory = restrictionService.GetRestrictionExcepts(br.Id)
                        };
            return Ok(query.ToList());
        }

        [HttpPatch("UpdateBuildingRestriction")]
        public IActionResult UpdateBuildingRestriction([FromBody] Restriction restriction)
        {   
            var restrictionExist = restrictionService.GetBuildingRestriction((int)restriction.BuildingId).ToList();
            if (restrictionExist.Any(b => b.CategoryId == restriction.CategoryId && b.Id != restriction.Id))
            {
                ModelState.AddModelError("Restriction", "The restriction already exists");
                return BadRequest(ModelState);
            }
            restrictionService.UpdateRestriction(restriction);
            return Ok();
        }

        [HttpPost]
        public IActionResult AddBuildingRestriction([FromBody] Restriction restriction)
        {
            var restrictionExist = restrictionService.GetBuildingRestriction((int)restriction.BuildingId).ToList();
            if (restrictionExist.Any(b => b.CategoryId == restriction.CategoryId))
            {
                ModelState.AddModelError("Restriction", "The restriction already exists");
                return BadRequest(ModelState);
            }
            restrictionService.AddRestriction(restriction);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRestriction(int id)
        {
            restrictionService.DeleteRestriction(id);
            return Ok();
        }

        [HttpGet("GetBuildingRestrictionById/{id}")]
        public IActionResult GetBuildingRestrictionById(int id)
        {
            var query = restrictionService.GetRestrictionById(id);
            var response = new {
                id = query.Id,
                buildingId = query.BuildingId,
                type = query.Type,
                category = _categoryService.GetCategoryById((int)query.CategoryId),
                arrCategory = restrictionService.GetRestrictionExcepts(query.Id)
            };

            return Ok(response);
        }
    }
}