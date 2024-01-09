using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;

namespace Hike.UseCases.Services.Interfaces;

public interface ITrailService
{
    Task<GetTrailResponse?> GetTrailById(Guid id);
    Task<GetTrailsResponse> GetTrails(string? searchValue, int page, int pageSize);
    Task<GetTrailsResponse?> GetUserTrails(int page, int pageSize);
    Task<GetTrailsResponse?> GetUserFavoriteTrails(int page, int pageSize);
    Task<AddTrailResponse> AddTrail(AddTrailRequest request);
    Task<AddTrailResponse> AddTrailToFavorites(Guid id);
    Task<bool> DeleteTrail(Guid id);
}