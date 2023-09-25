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
        var trail = await _context.Trails.FindAsync(id);
        return trail?.ToTrailModel();
    }

    public async Task<IEnumerable<TrailModel>> SearchTrailByTitle(string searchValue)
    {
        var trails = await _context.Trails.Where(t => t.Title.Contains(searchValue)).ToArrayAsync();
        return trails.Select(t => t.ToTrailModel());
    }
}