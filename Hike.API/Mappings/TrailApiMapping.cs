using AutoMapper;
using Hike.API.DTO;
using Hike.Domain.Entities;
using Hike.UseCases.Responses;

namespace Hike.API.Mappings;

public class TrailApiMapping : Profile
{
    public TrailApiMapping()
    {
        CreateMap<TrailDTO, GetTrailResponse>();
        CreateMap<GetTrailResponse, TrailDTO>();
    }
}