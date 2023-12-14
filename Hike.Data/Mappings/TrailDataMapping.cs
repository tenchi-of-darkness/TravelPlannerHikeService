using AutoMapper;
using Hike.Data.DBO;
using Hike.Domain.Entities;

namespace Hike.Data.Mappings;

public class TrailDataMapping : Profile
{
    public TrailDataMapping()
    {
        CreateMap<TrailDBO, TrailEntity>();
        CreateMap<TrailEntity, TrailDBO>();
    }
}