using AutoMapper;
using Hike.API.DTO;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;
using Hike.UseCases.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hike.API.Controllers;

[ApiController]
[Route("api/trail")]
public class TrailController : ControllerBase
{
    private readonly ILogger<TrailController> _logger;
    private readonly IMapper _mapper;
    private readonly ITrailService _service;

    public TrailController(ILogger<TrailController> logger, ITrailService service, IMapper mapper)
    {
        _logger = logger;
        _service = service;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TrailDTO>> GetTrailById([FromRoute] Guid id)
    {
        var trail = await _service.GetTrailById(id);
        if (trail == null) return NotFound();

        return Ok(_mapper.Map<TrailDTO>(trail));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrailDTO>>> GetTrails([FromQuery] SearchTrailsRequest request)
    {
        var response = await _service.GetTrails(request.SearchValue, request.Page, request.PageSize);
        return Ok(_mapper.Map<TrailDTO[]>(response.Trails));
    }

    [HttpGet("user")]
    public async Task<ActionResult<IEnumerable<TrailDTO>>> GetUserTrails([FromQuery] GetTrailsRequest request)
    {
        var response = await _service.GetUserTrails(request.Page, request.PageSize);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<TrailDTO[]>(response.Trails));
    }

    [HttpGet("favorite")]
    public async Task<ActionResult<IEnumerable<TrailDTO>>> GetFavoriteTrails([FromQuery] GetTrailsRequest request)
    {
        var response = await _service.GetUserFavoriteTrails(request.Page, request.PageSize);

        if (response == null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<TrailDTO[]>(response.Trails));
    }

    [HttpPost]
    public async Task<ActionResult<TrailResponse>> AddTrail([FromBody] AddTrailRequest request)
    {
        var response = await _service.AddTrail(request);
        return response.FailureType switch
        {
            null => Ok(),
            FailureType.User => BadRequest(response),
            _ => StatusCode(500, response)
        };
    }

    [HttpPost("{id:guid}/favorite")]
    public async Task<ActionResult<TrailResponse>> AddTrailToFavorites([FromRoute] Guid id)
    {
        var response = await _service.AddTrailToFavorites(id);
        return response.FailureType switch
        {
            null => Ok(),
            FailureType.User => BadRequest(response),
            _ => StatusCode(500, response)
        };
    }
    
    [HttpDelete("{id:guid}/favorite")]
    public async Task<ActionResult<TrailResponse>> RemoveTrailFromFavorites([FromRoute] Guid id)
    {
        var response = await _service.RemoveTrailFromFavorites(id);
        return response.FailureType switch
        {
            null => Ok(),
            FailureType.User => BadRequest(response),
            _ => StatusCode(500, response)
        };
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTrail([FromRoute] Guid id)
    {
        if (await _service.DeleteTrail(id))
        {
            return Ok();
        }

        return NotFound();
    }
}