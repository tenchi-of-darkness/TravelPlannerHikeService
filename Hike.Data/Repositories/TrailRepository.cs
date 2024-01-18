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
        var trail = await _context.Trails.Where(x => x.Id == id).FirstOrDefaultAsync();
        return _mapper.Map<TrailEntity>(trail);
    }

    public async Task<IEnumerable<TrailEntity>> SearchTrailByTitle(string? searchValue, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var query = _context.Trails.AsQueryable();

        if (searchValue != null) query = query.Where(t => t.Title.Contains(searchValue));

        return _mapper.Map<TrailEntity[]>(await query.OrderByDescending(t => t.Id).Skip(skip)
            .Take(pageSize).ToArrayAsync());
    }

    public async Task<IEnumerable<TrailEntity>> SearchTrailByUser(string userId, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var query = _context.Trails.Where(t => t.OwnerUserId == userId);

        return _mapper.Map<TrailEntity[]>(await query.OrderByDescending(t => t.Id).Skip(skip)
            .Take(pageSize).ToArrayAsync());
    }

    public async Task<IEnumerable<TrailEntity>> SearchFavoriteTrailsByUser(string userId, int page, int pageSize)
    {
        var skip = (page - 1) * pageSize;
        var query = _context.Users.Where(t => t.Id == userId).SelectMany(x => x.FavoriteTrails);

        return _mapper.Map<TrailEntity[]>(await query.OrderByDescending(t => t.Id).Skip(skip)
            .Take(pageSize).ToArrayAsync());
    }

    public async Task<bool> AddTrail(TrailEntity entity)
    {
        _context.Trails.Add(_mapper.Map<TrailDBO>(entity));
        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<bool> UpdateTrail(TrailEntity entity)
    {
        var oldEntity = _context.Trails.Local.FirstOrDefault(x => entity.Id==x.Id);
        if (oldEntity != null)
        {
            _context.Entry(oldEntity).State = EntityState.Detached;
        }
        
        _context.Trails.Update(_mapper.Map<TrailDBO>(entity));
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> AddTrailToFavorites(string userId, Guid id)
    {
        var user = await _context.Users.Include(x => x.FavoriteTrails).Where(x => x.Id == userId)
            .SingleOrDefaultAsync();
        var trail = await _context.Trails.FindAsync(id);

        if (trail == null)
        {
            return false;
        }

        if (user == null)
        {
            var newUser = new UserDBO
            {
                Id = userId,
                FavoriteTrails = new List<TrailDBO>
                {
                    trail
                }
            };
            _context.Users.Add(newUser);
            return await _context.SaveChangesAsync() >= 1;
        }

        user.FavoriteTrails.Add(trail);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> RemoveTrailFromFavorites(string userId, Guid id)
    {
        var user = await _context.Users.Include(x => x.FavoriteTrails).Where(x => x.Id == userId)
            .SingleOrDefaultAsync();

        var trail = user?.FavoriteTrails.FirstOrDefault(x => x.Id == id);
        if (trail == null)
        {
            return false;
        }

        user!.FavoriteTrails.Remove(trail);
        return await _context.SaveChangesAsync() >= 1;
    }

    public async Task<bool> DeleteTrail(Guid id)
    {
        return await _context.Trails.Where(x => x.Id == id).ExecuteDeleteAsync() == 1;
    }
}