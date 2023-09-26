using Hike.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Hike.Logic.Services.Interfaces;

namespace Hike.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrailController : ControllerBase
{
    private readonly ILogger<TrailController> _logger;
    private readonly ITrailService _service;

    public TrailController(ILogger<TrailController> logger, ITrailService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet(Name = "GetTrailById")]
    public async Task<ActionResult<TrailModel?>> GetTrailById([FromQuery] Guid id)
    {
        return Ok(await _service.GetTrailById(id));
    }

    [HttpGet(Name = "SearchTrailByTitle")]
    public async Task<ActionResult<IEnumerable<TrailModel>>> SearchTrailByTitle([FromQuery] string searchValue, int page, int pageSize)
    {
        return Ok(await _service.SearchTrailByTitle(searchValue, page, pageSize));
    }
}