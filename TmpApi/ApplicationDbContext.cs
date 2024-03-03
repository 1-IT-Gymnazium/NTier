using Microsoft.EntityFrameworkCore;

namespace TmpApi;

public class ApplicationDbContext : DbContext
{
    public DbSet<Home> Homes { get; set; }
}
public class Home
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameSlug { get; set; } = null!;
    public Guid ConnectionId { get; set; }
}
