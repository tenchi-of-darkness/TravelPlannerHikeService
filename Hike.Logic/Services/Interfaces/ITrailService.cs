using Hike.API.Models.Responses;
using Hike.Logic.Entities;

namespace Hike.Logic.Services.Interfaces;

public interface ITrailService
{
    Task<TrailEntity?> GetTrailById(Guid id);

    Task<IEnumerable<TrailEntity>> GetTrails(string? searchValue, int page, int pageSize);
    Task<AddTrailResponse> AddTrail(TrailEntity entity);
    Task<bool> DeleteTrail(Guid id);
}