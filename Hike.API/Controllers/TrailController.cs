using Hike.API.Models;
using Hike.API.Models.Responses;
using Hike.Logic.Entities;
using Hike.Logic.Requests.Trail;
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
    public async Task<ActionResult<TrailModel>> GetTrailById([FromRoute] Guid id)
    {
        var trail = await _service.GetTrailById(id);
        if (trail == null)
        {
            return NotFound();
        }

        return Ok(trail);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrailModel>>> GetTrails([FromQuery]GetTrailsRequest request)
    {
        return Ok(await _service.GetTrails(request.SearchValue, request.Page, request.PageSize));
    }

    [HttpPost]
    public async Task<ActionResult<AddTrailResponse>> AddTrail([FromBody] TrailEntity entity)
    {
        AddTrailResponse response = await _service.AddTrail(entity);
        if (response.FailureReason == null)
        {
            return Ok();
        }

        if (response.FailureType == FailureType.User)
        {
            return BadRequest(response);
        }
        
        return StatusCode(500, response);
    }
}