using Hike.Logic.Models;
using Hike.Logic.Models.Responses;

namespace Hike.Logic.Services.Interfaces;

public interface ITrailService
{
    Task<TrailModel?> GetTrailById(Guid id);

    Task<IEnumerable<TrailModel>> GetTrails(string? searchValue, int page, int pageSize);
    Task<AddTrailResponse> AddTrail(TrailModel model);
}