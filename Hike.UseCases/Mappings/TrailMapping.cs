using AutoMapper;
using Hike.Domain.Entities;
using Hike.UseCases.Requests.Trail;
using Hike.UseCases.Responses;

namespace Hike.UseCases.Mappings;

public class TrailMapping : Profile
{
    public TrailMapping()
    {
        CreateMap<AddTrailRequest, TrailEntity>();
        CreateMap<UpdateTrailRequest, TrailEntity>();
        CreateMap<TrailEntity, TrailResponse>();
        CreateMap<TrailEntity, GetTrailResponse>();
    }
}