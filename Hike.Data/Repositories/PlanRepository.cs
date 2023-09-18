using Hike.Logic.Repositories.Interfaces;

namespace Hike.Data.Repositories;

public class PlanRepository : IPlanRepository
{
    public string DaPlan()
    {
        return "DAPLAN";
    }
}