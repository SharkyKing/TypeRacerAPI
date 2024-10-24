using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameBase> Games { get; set; }
        public DbSet<PlayerBase> Players { get; set; }
        public DbSet<GameLevelBase> GameLevel { get; set; }
        public DbSet<GameTypeBase> GameType { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameBase>()
                .HasMany(g => g.Players)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId);

            modelBuilder.Entity<GameBase>()
                .HasOne(g => g.GameLevel)
                .WithMany(gl => gl.Games)
                .HasForeignKey(g => g.GameLevelId);

            modelBuilder.Entity<GameBase>()
                .HasOne(g => g.GameType)
                .WithMany(gt => gt.Games)
                .HasForeignKey(g => g.GameTypeId);

            modelBuilder.Entity<GameLevelBase>().HasData(
                new GameLevelBase { Id = 1, GameLevelName = "Beginner" },
                new GameLevelBase { Id = 2, GameLevelName = "Normal" },
                new GameLevelBase { Id = 3, GameLevelName = "Advanced" }
            );

            modelBuilder.Entity<GameTypeBase>().HasData(
                new GameTypeBase { Id = 1, GameTypeName = "TimeAttack" },
                new GameTypeBase { Id = 2, GameTypeName = "FluentType" }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
