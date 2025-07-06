using Microsoft.EntityFrameworkCore;
using AudioWeb.Client.Models;

namespace AudioWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AudiometerModel> Audiometros { get; set; }
    }
}
