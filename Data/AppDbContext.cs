using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TypeRacerAPI.BaseClasses;

namespace TypeRacerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameBase> Games { get; set; }
        public DbSet<PlayerBase> Players { get; set; }
        public DbSet<GameLevelBase> GameLevel { get; set; }
        public DbSet<GameTypeBase> GameType { get; set; }
        public DbSet<PlayerPowerBase> PlayerPower { get; set; }
        public DbSet<PlayerPowerUse> PlayerPowerUses { get; set; }
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

            modelBuilder.Entity<PlayerPowerUse>()
                .HasOne(p => p.Player)
                .WithMany(player => player.PlayerPowerUses)
                .HasForeignKey(p => p.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerPowerUse>()
                .HasOne(pp => pp.PlayerPower)
                .WithMany(ppBase => ppBase.PlayerPowerUses)
                .HasForeignKey(pp => pp.PlayerPowerId);

            modelBuilder.Entity<GameLevelBase>().HasData(
                new GameLevelBase { Id = 1, GameLevelName = "Beginner" },
                new GameLevelBase { Id = 2, GameLevelName = "Normal" },
                new GameLevelBase { Id = 3, GameLevelName = "Advanced" }
            );

            modelBuilder.Entity<GameTypeBase>().HasData(
                new GameTypeBase { Id = 1, GameTypeName = "TimeAttack" },
                new GameTypeBase { Id = 2, GameTypeName = "FluentType" }
            );

            modelBuilder.Entity<PlayerPowerBase>().HasData(
                new PlayerPowerBase { Id = 1, PlayerPowerName = "Freeze", PlayerPowerKey = "F", ImagePath = "/images/freeze.png" },
                new PlayerPowerBase { Id = 2, PlayerPowerName = "Rewind", PlayerPowerKey = "R", ImagePath = "/images/rewind.png" },
                new PlayerPowerBase { Id = 3, PlayerPowerName = "Invisible", PlayerPowerKey = "I", ImagePath = "/images/invisible.png" }
            );

            base.OnModelCreating(modelBuilder);
        }

        public void InitializeDatabase()
        {

        }
    }
}
