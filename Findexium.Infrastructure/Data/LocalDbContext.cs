using Microsoft.EntityFrameworkCore;
using Findexium.Domain.Models;



namespace Findexium.Infrastructure.Data
{
    public class LocalDbContext : DbContext
    {
        public LocalDbContext(DbContextOptions<LocalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //referencer les entités
        public DbSet<User> Users { get; set; }
        public DbSet<BidList> Bids { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Trade> Trades { get; set; }





    }
}