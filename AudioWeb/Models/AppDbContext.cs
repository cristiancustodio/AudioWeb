using Microsoft.EntityFrameworkCore;
using AudioWeb.Shared.Models;

namespace AudioWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AudiometroModel> Audiometros { get; set; }
    }
}
