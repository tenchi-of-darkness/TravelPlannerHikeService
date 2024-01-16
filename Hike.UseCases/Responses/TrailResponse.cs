namespace Hike.UseCases.Responses;

public record TrailResponse(FailureType? FailureType = null, string? FailureReason = null);

public enum FailureType
{
    User,
    Server
}