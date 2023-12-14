using AutoMapper;
using Hike.API.DTO;
using Hike.Domain.Entities;

namespace Hike.API.Mappings;

public class TrailApiMapping : Profile
{
    public TrailApiMapping()
    {
        CreateMap<TrailDTO, TrailEntity>();
        CreateMap<TrailEntity, TrailDTO>();
    }
}