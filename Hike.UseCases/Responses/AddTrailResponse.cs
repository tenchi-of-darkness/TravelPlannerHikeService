namespace Hike.UseCases.Responses;

public record AddTrailResponse(FailureType? FailureType = null, string? FailureReason = null);

public enum FailureType
{
    User,
    Server
}