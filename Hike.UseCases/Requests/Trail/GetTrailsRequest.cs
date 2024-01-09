namespace Hike.UseCases.Requests.Trail;

public record GetTrailsRequest(int Page = 1, int PageSize = 10);