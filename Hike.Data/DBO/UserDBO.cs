using Hike.Domain.Entities;

namespace Hike.Data.DBO;

public class UserDBO
{
    public string Id { get; set; }
    public ICollection<TrailDBO> FavoriteTrails { get; set; }
}