using Microsoft.AspNetCore.Mvc;
using OsrsTool.Domain.DTOs;
using OsrsTool.Domain.Enums;
using OsrsTool.Domain.Interfaces;

namespace OsrsTool.Server.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GearController : ControllerBase
    {
        private readonly IGearService _gearService;

        public GearController(IGearService gearService)
        {
            _gearService = gearService;
        }

        [HttpPost("upgrades")]
        public async Task<IActionResult> GetGearUpgrades([FromBody] GearProgressionRequestDto request)
        {
            var upgrades = await _gearService.GetGearUpgradesAsync(request.CombatStyle, request.OwnedItemIds);
            return Ok(upgrades);
        }

        [HttpGet("bis/{combatStyle}")]
        public async Task<IActionResult> GetBisProgression(CombatStyle combatStyle)
        {
            var progression = await _gearService.GetBisProgressionAsync(combatStyle);
            return Ok(progression);
        }
    }
}
