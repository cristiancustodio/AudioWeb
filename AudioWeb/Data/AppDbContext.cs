// AudioWeb/Data/AppDbContext.cs
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AudioWeb.Models;
using AudioWeb.Shared.Models;

namespace AudioWeb.Data
{
    // CORRETO: ApiAuthorizationDbContext precisa de TUser, TRole e TKey
    public class AppDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public AppDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }

        public DbSet<AudiometroModel> Audiometros { get; set; }        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Configurações adicionais de modelo, se houver
        }
    }
}
