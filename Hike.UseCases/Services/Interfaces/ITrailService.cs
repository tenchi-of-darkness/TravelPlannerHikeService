using Hike.Domain.Entities;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;

namespace Hike.UseCases.Services.Interfaces;

public interface ITrailService
{
    Task<TrailEntity?> GetTrailById(Guid id);

    Task<IEnumerable<TrailEntity>> GetTrails(string? searchValue, int page, int pageSize);
    Task<AddTrailResponse> AddTrail(AddTrailRequest request);
    Task<bool> DeleteTrail(Guid id);
}