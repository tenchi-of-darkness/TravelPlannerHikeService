using Hike.Logic.Repositories.Interfaces;
using Hike.Logic.Services.Interfaces;

namespace Hike.Logic.Services;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepository;

    public PlanService(IPlanRepository planRepository)
    {
        _planRepository = planRepository;
    }
    public string DoPlan()
    {
        return _planRepository.DaPlan();
    }
}