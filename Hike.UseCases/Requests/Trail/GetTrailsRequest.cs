namespace Hike.UseCases.Requests.Trail;

public record GetTrailsRequest(string? SearchValue, int Page = 1, int PageSize = 10);