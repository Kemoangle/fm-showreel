using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Service;
using Microsoft.AspNetCore.Mvc;
using Showreel.Models;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RulesController : ControllerBase
    {
        private readonly IRuleService ruleService;
        public RulesController(IRuleService _ruleService)
        {
            ruleService = _ruleService;
        }

        [HttpPatch("UpdateDoNotPlay/{id}")]
        public IActionResult UpdateDoNotPlay(int id, [FromBody] Building[] buildings)
        {
            ruleService.UpdateDoNotPlay(id,buildings);

            return Ok();
        }


        [HttpPatch("UpdateNoBackToBack/{id}")]
        public IActionResult UpdateNoBackToBack(int id, [FromBody] Category[] categories)
        {
            ruleService.UpdateNoBackToBack(id,categories);

            return Ok();
        }
    }
}