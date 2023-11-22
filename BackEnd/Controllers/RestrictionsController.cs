using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;
using Showreel.Service;

namespace Showreel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestrictionsController : ControllerBase
    {
        private readonly IRestrictionService _restrictionService;

        public RestrictionsController(IRestrictionService restrictionService)
        {
            _restrictionService = restrictionService;
        }

        [HttpGet]
        public IActionResult GetAllRestrictions()
        {
            var restriction = _restrictionService.GetAllRestrictions();
            return Ok(restriction);
        }

        [HttpGet("{id}")]
        public IActionResult GetRestrictionById(int id)
        {
            var restriction = _restrictionService.GetRestrictionById(id);
            if (restriction == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }
            return Ok(restriction);
        }

        [HttpPost]
        public IActionResult CreateRestriction([FromBody] Restriction restriction)
        {
            var restrictionExist = _restrictionService.GetAllRestrictions().ToList();
            if (restrictionExist.Any(b => b.RestrictionName.ToUpper() == restriction.RestrictionName.ToUpper()))
            {
                ModelState.AddModelError("Name", "The restriction name already exists");
                return BadRequest(ModelState);
            }
            _restrictionService.AddRestriction(restriction);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateRestriction(int id, [FromBody] Restriction restriction)
        {
            var existingRestriction = _restrictionService.GetRestrictionById(id);
            if (existingRestriction == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }

            existingRestriction.RestrictionName = restriction.RestrictionName;
            _restrictionService.UpdateRestriction(existingRestriction);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRestriction(int id)
        {
            var existingRestriction = _restrictionService.GetRestrictionById(id);
            if (existingRestriction == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }
            _restrictionService.DeleteRestriction(id);
            return Ok();
        }

        [HttpGet("building/{id}")]
        public IActionResult GetRestrictionByBuildingId(int id)
        {
            var restriction = _restrictionService.GetRestrictionByBuildingId(id);
            if (restriction == null)
            {
                ModelState.AddModelError("Id", "The Id not found");

                return NotFound(ModelState);
            }
            return Ok(restriction);
        }
    }
}
