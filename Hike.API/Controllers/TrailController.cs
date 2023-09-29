using Hike.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Hike.Logic.Services.Interfaces;

namespace Hike.API.Controllers;

[ApiController]
[Route("api/trails")]
public class TrailController : ControllerBase
{
    private readonly ILogger<TrailController> _logger;
    private readonly ITrailService _service;

    public TrailController(ILogger<TrailController> logger, ITrailService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrailModel?>> GetTrailById([FromRoute] Guid id)
    {
        return Ok(await _service.GetTrailById(id));
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<TrailModel>>> SearchTrailByTitle([FromQuery] string searchValue,
        int page, int pageSize)
    {
        return Ok(await _service.SearchTrailByTitle(searchValue, page, pageSize));
    }

    [HttpPost]
    public async Task<IActionResult> AddTrail([FromBody] TrailModel model)
    {
        bool success = await _service.AddTrail(model);
        if (success)
        {
            return Ok();
        }

        return BadRequest();
    }
}