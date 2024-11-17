using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.AccessControl;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Singleton.LevelTexts;
using TypeRacerAPI.Services;

namespace TypeRacerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<GameClass> Games { get; set; }
        public DbSet<PlayerClass> Players { get; set; }
        public DbSet<BaseClasses.GameLevelClass> GameLevel { get; set; }
        public DbSet<GameTypeClass> GameType { get; set; }
        public DbSet<PlayerPowerClass> PlayerPower { get; set; }
        public DbSet<PlayerPowerUseClass> PlayerPowerUses { get; set; }
        public DbSet<WordsClass> Words { get; set; }
        public DbSet<GameLogClass> GameLog { get; set; }
        public DbSet<LogTypeClass> LogType { get; set; }
        public DbSet<WordsStyleClass> WordsStyle { get; set; }
        public DbSet<PlayerGameResultTypeClass> PlayerGameResultType { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameClass>()
                .HasMany(g => g.Players)
                .WithOne(p => p.Game)
                .HasForeignKey(p => p.GameId);

            modelBuilder.Entity<WordsStyleClass>()
                .HasMany(g => g.Players)
                .WithOne(p => p.WordsStyle)
                .HasForeignKey(p => p.WordsStyleId);

            modelBuilder.Entity<GameClass>()
                .HasOne(g => g.GameLevel)
                .WithMany(gl => gl.Games)
                .HasForeignKey(g => g.GameLevelId);

            modelBuilder.Entity<WordsClass>()
                .HasOne(g => g.GameLevel)
                .WithMany(gl => gl.Words)
                .HasForeignKey(g => g.GameLevelId);

            modelBuilder.Entity<GameClass>()
                .HasOne(g => g.GameType)
                .WithMany(gt => gt.Games)
                .HasForeignKey(g => g.GameTypeId);

            modelBuilder.Entity<PlayerPowerUseClass>()
                .HasOne(p => p.Player)
                .WithMany(player => player.PlayerPowerUses)
                .HasForeignKey(p => p.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameLogClass>()
               .HasOne(p => p.Game)
               .WithMany(game => game.GameLog)
               .HasForeignKey(p => p.GameId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GameLogClass>()
               .HasOne(p => p.Player)
               .WithMany(player => player.GameLog)
               .HasForeignKey(p => p.PlayerId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<GameLogClass>()
               .HasOne(p => p.LogType)
               .WithMany(logType => logType.GameLog)
               .HasForeignKey(p => p.LogTypeId)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<PlayerPowerUseClass>()
                .HasOne(pp => pp.PlayerPower)
                .WithMany(ppBase => ppBase.PlayerPowerUses)
                .HasForeignKey(pp => pp.PlayerPowerId);

            modelBuilder.Entity<GameLevelClass>().HasData(
                SeedData.GameLevelsSeed
            );

            modelBuilder.Entity<PlayerGameResultTypeClass>().HasData(
                SeedData.PlayerGameResultTypesSeed   
            );

            modelBuilder.Entity<LogTypeClass>().HasData(
                SeedData.LogTypesSeed
            );

            modelBuilder.Entity<GameTypeClass>().HasData(
                SeedData.GameTypesSeed
            );

            modelBuilder.Entity<PlayerPowerClass>().HasData(
                SeedData.PlayerPowersSeed
            );

            modelBuilder.Entity<WordsStyleClass>().HasData(
               SeedData.WordsStylesSeed
            ); 

            modelBuilder.Entity<BaseClasses.GameLevelClass>()
                .Property(gl => gl.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<GameTypeClass>()
                .Property(gt => gt.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PlayerPowerClass>()
                .Property(pp => pp.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PlayerPowerUseClass>()
                .Property(ppu => ppu.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<GameClass>()
                .Property(g => g.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PlayerClass>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<WordsClass>()
                .Property(w => w.Id)
                .ValueGeneratedOnAdd();



            var wordsData = LevelTexts.GetInstance().LoadTextsFromJson();

            modelBuilder.Entity<WordsClass>().HasData(wordsData);

            base.OnModelCreating(modelBuilder);
        }

        public void InitializeDatabase()
        {

        }
    }
}
