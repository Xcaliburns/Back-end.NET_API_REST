using Microsoft.EntityFrameworkCore;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;



namespace Findexium.Infrastructure.Data
{
    public class LocalDbContext : IdentityDbContext<IdentityUser>
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //referencer les entités

        //TODO : voir avec Laala : les champs de la table USER sont ils pour la plupart déjà inclus dans IdentityUser ?
        public DbSet<User> Users { get; set; }
        public DbSet<BidList> Bids { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }





    }
}