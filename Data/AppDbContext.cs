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
                new GameLevelClass { Id = 1, GameLevelName = "Beginner" },
                new GameLevelClass { Id = 2, GameLevelName = "Normal" },
                new GameLevelClass { Id = 3, GameLevelName = "Advanced" }
            );

            modelBuilder.Entity<LogTypeClass>().HasData(
                new LogTypeClass { Id = 1, LogTypeName = "Error" },
                new LogTypeClass { Id = 2, LogTypeName = "GameLog" },
                new LogTypeClass { Id = 3, LogTypeName = "Information" }
            );

            modelBuilder.Entity<GameTypeClass>().HasData(
                new GameTypeClass { Id = 1, GameTypeName = "TimeAttack" },
                new GameTypeClass { Id = 2, GameTypeName = "FluentType" }
            );

            modelBuilder.Entity<PlayerPowerClass>().HasData(
                new PlayerPowerClass { Id = 1, PlayerPowerName = "Freeze", PlayerPowerKey = "F", ImagePath = "/images/freeze.png", CooldownTime = 10, IsTimedPower = true},
                new PlayerPowerClass { Id = 2, PlayerPowerName = "Rewind", PlayerPowerKey = "R", ImagePath = "/images/rewind.png", CooldownTime = 5, IsTimedPower = false},
                new PlayerPowerClass { Id = 3, PlayerPowerName = "Invisible", PlayerPowerKey = "I", ImagePath = "/images/invisible.png", CooldownTime = 15, IsTimedPower = true}
            );

            modelBuilder.Entity<WordsStyleClass>().HasData(
               new WordsStyleClass { Id = 1, StyleName = "BoldDecorator", fontFamily = "Arial, sans-serif", fontWeight = "bold", fontStyle = null },
               new WordsStyleClass { Id = 2, StyleName = "ItalicDecorator", fontFamily = "Georgia, serif", fontWeight = null, fontStyle = null },
               new WordsStyleClass { Id = 3, StyleName = "FancyFontDecorator", fontFamily = "Courier New, monospace", fontWeight = "normal", fontStyle = "normal" }
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
