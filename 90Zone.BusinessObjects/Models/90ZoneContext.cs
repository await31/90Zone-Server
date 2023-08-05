using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace _90Zone.BusinessObjects.Models {
    public class _90ZoneDbContext : IdentityDbContext {

        public _90ZoneDbContext() { }
        public _90ZoneDbContext(DbContextOptions<_90ZoneDbContext> options) : base(options) { }
        public virtual DbSet<Club> Clubs { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Player> Players { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }

        private string GetConnectionString() {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config["ConnectionStrings:DefaultConnection"];

            return strConn;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
