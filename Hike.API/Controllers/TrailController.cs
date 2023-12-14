using AutoMapper;
using Hike.API.DTO;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hike.API.Controllers;

[ApiController]
[Route("api/trail")]
public class TrailController : ControllerBase
{
    private readonly ILogger<TrailController> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailService _service;

    public TrailController(ILogger<TrailController> logger, ITrailService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetTrailResponse>> GetTrailById([FromRoute] Guid id)
    {
        var trail = await _service.GetTrailById(id);
        if (trail == null) return NotFound();

        return Ok(trail);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrailDTO>>> GetTrails([FromQuery] GetTrailsRequest request)
    {
        return Ok(await _service.GetTrails(request.SearchValue, request.Page, request.PageSize));
    }

    [HttpPost]
    public async Task<ActionResult<AddTrailResponse>> AddTrail([FromBody] AddTrailRequest request)
    {
        var response = await _service.AddTrail(request);
        if (response.FailureType == null) return Ok();

        if (response.FailureType == FailureType.User) return BadRequest(response);

        return StatusCode(500, response);
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTrail(Guid id)
    {
        if (await _service.DeleteTrail(id)) NotFound();
        return new NotFoundResult();
    }
}