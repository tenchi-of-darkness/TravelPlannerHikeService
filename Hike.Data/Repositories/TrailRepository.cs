using Hike.Data.DbContext;
using Hike.Logic.Entities;
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

    public async Task<TrailEntity?> GetTrailById(Guid id)
    {
        var trail = await _context.Trails.FindAsync(id);
        return trail;
    }

    public async Task<bool> AddTrail(TrailEntity entity)
    {
        _context.Trails.Add(entity);
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _context.Trails.Where(x => x.Id == id).ExecuteDeleteAsync() == 1;
    }

    public async Task<IEnumerable<TrailEntity>> SearchTrailByTitle(string? searchValue, int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;
        var query = _context.Trails.AsQueryable();

        if (searchValue != null)
        {
            query = query.Where(t => t.Title.Contains(searchValue));
        }

        return await query.Skip(skip)
            .Take(pageSize).ToArrayAsync();
    }
}