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

        [HttpPatch("UpdateBuildingRestriction")]
        public IActionResult UpdateBuildingRestriction([FromBody] BuildingRestriction buildingRestriction)
        {   
            var restrictionExist = restrictionService.GetBuildingRestriction((int)buildingRestriction.BuildingId).ToList();
            if (restrictionExist.Any(b => b.CategoryId == buildingRestriction.CategoryId && b.Id != buildingRestriction.Id))
            {
                ModelState.AddModelError("Restriction", "The restriction already exists");
                return BadRequest(ModelState);
            }
            var response = restrictionService.UpdateBuildingRestriction(buildingRestriction);
            return Ok(response);
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

        [HttpDelete("{id}")]
        public IActionResult DeleteRestriction(int id)
        {
            restrictionService.DeleteRestriction(id);
            return Ok();
        }

        [HttpGet("GetBuildingRestrictionById/{id}")]
        public IActionResult GetBuildingRestrictionById(int id)
        {
            var query = restrictionService.GetBuildingRestrictionById(id);
            var response = new {
                id = query.Id,
                buildingId = query.BuildingId,
                type = query.Type,
                categoryId = query.CategoryId,
                category = _categoryService.GetCategoryById((int)query.CategoryId),
                except = restrictionService.GetVideoExcept(query.Id)
            };

            return Ok(response);
        }
    }
}