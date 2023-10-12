using Hike.Data.Entities;
using Hike.Logic.Models;
using Hike.Logic.Models.Requests.Trail;
using Hike.Logic.Models.Responses;
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
    public async Task<ActionResult<AddTrailResponse>> AddTrail([FromBody] TrailModel model)
    {
        AddTrailResponse response = await _service.AddTrail(model);
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