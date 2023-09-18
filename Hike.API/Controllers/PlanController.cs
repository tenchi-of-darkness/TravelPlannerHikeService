using Microsoft.AspNetCore.Mvc;
using Hike.Logic.Services.Interfaces;

namespace Hike.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlanController : ControllerBase
{
    private readonly ILogger<PlanController> _logger;
    private readonly IPlanService _service;

    public PlanController(ILogger<PlanController> logger, IPlanService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet(Name = "DoPlan")]
    public string DoPlan()
    {
        _logger.LogDebug("Did plan.");
        return _service.DoPlan();
    }
}