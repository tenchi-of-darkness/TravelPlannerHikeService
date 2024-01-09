namespace Hike.UseCases.Requests.Trail;

public record SearchTrailsRequest(string? SearchValue, int Page = 1, int PageSize = 10);