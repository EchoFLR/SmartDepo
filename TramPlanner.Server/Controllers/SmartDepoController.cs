using Microsoft.AspNetCore.Mvc;
using TramPlanner.Server.Services;

namespace TramPlanner.Server.Controllers;

/// <summary>
/// API Controller for managing the Smart Depot.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SmartDepoController : ControllerBase
{
    private readonly SmartDepoService _smartDepoService;

    public SmartDepoController(SmartDepoService smartDepoService)
    {
        _smartDepoService = smartDepoService;
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize([FromBody] object tramJson)
    {
        try
        {
            await _smartDepoService.InitializeAsync(tramJson.ToString());
            return Ok(new { Message = "SmartDepo initialized successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("trams")]
    public async Task<IActionResult> GetTrams()
    {
        try
        {
            var trams = await _smartDepoService.GetTramsAsync();
            return Ok(trams);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPost("assignMission")]
    public async Task<IActionResult> AssignMissionAsync([FromBody] object mission)
    {
        try
        {
            await _smartDepoService.AssignMissionAsync(mission.ToString());
            return Ok(new { Message = "Mission assigned successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }


    [HttpPost("reset")]
    public async Task<IActionResult> ResetData()
    {
        try
        {
            await _smartDepoService.ResetData();
            return Ok(new { Message = "Tram data has been reset successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
