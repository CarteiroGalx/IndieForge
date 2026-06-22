using Microsoft.EntityFrameworkCore;

namespace IndieForge.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
