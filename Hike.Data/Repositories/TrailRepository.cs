using AutoMapper;
using Hike.Data.DbContext;
using Hike.Data.DBO;
using Hike.Domain.Entities;
using Hike.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hike.Data.Repositories;

public class TrailRepository : ITrailRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TrailRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TrailEntity?> GetTrailById(Guid id)
    {
        var trail = await _context.Trails.Where(x=>x.Id==id).FirstOrDefaultAsync();
        return _mapper.Map<TrailEntity>(trail);
    }

    public async Task<bool> AddTrail(TrailEntity entity)
    {
        _context.Trails.Add(_mapper.Map<TrailDBO>(entity));
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _context.Trails.Where(x => x.Id == id).ExecuteDeleteAsync() == 1;
    }

    public async Task<IEnumerable<TrailEntity>> SearchTrailByTitle(string? searchValue, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var query = _context.Trails.AsQueryable();

        if (searchValue != null) query = query.Where(t => t.Title.Contains(searchValue));

        return _mapper.Map<TrailEntity[]>(await query.Skip(skip)
            .Take(pageSize).ToArrayAsync());
    }
}