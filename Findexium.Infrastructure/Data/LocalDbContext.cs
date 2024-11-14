using Microsoft.EntityFrameworkCore;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Findexium.Infrastructure.Models;



namespace Findexium.Infrastructure.Data
{
    public class LocalDbContext : IdentityDbContext<User>
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<BidDto>().HasKey(b => b.BidListId); // Define primary key
            builder.Entity<CurvePointsDto>().HasKey(c => c.Id);
            builder.Entity<RatingDto>().HasKey(r => r.Id);
            builder.Entity<RuleNameDto>().HasKey(r => r.Id);
            builder.Entity<TradeDto>().HasKey(t => t.TradeId);
        }

        public DbSet<BidDto> Bids { get; set; }
        public DbSet<CurvePointsDto> CurvePoints { get; set; }
        public DbSet<RatingDto> Ratings { get; set; }
        public DbSet<RuleNameDto> RuleNames { get; set; }
        public DbSet<TradeDto> Trades { get; set; }
    }
}