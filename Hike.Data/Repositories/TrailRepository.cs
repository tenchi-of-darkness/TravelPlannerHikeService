using Hike.Data.DbContext;
using Hike.Data.Entities;
using Hike.Logic.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hike.Data.Repositories;

public class TrailRepository : ITrailRepository
{
    private readonly ApplicationDbContext _context;

    public TrailRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TrailModel?> GetTrailById(Guid id)
    {
        TrailEntity? trail = await _context.Trails.FindAsync(id);
        return trail?.ToTrailModel();
    }

    public async Task<bool> AddTrail(TrailModel model)
    {
        _context.Trails.Add(new TrailEntity(model));
        return await _context.SaveChangesAsync()==1;
    }

    public async Task<IEnumerable<TrailModel>> SearchTrailByTitle(string searchValue, int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;
        TrailEntity[] trails = await _context.Trails.Where(t => t.Title.Contains(searchValue)).Skip(skip).Take(pageSize)
            .ToArrayAsync();
        return trails.Select(t => t.ToTrailModel());
    }
}