namespace Hike.UseCases.Responses;

public record GetTrailsResponse(IEnumerable<GetTrailResponse> Trails);