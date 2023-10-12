namespace Hike.Logic.Models.Responses;

public record AddTrailResponse(FailureType? FailureType = null, string? FailureReason = null);

public enum FailureType
{
    User,
    Server
}