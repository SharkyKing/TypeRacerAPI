using System.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using TypeRacerAPI.Controllers;
using TypeRacerAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TypeRacerAPI.Hubs;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Controllers.ControllerHelperClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Command;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;

namespace TypeRacerAPITest
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly AppDbContext _context;
        private readonly GameController _controller;

        public GameControllerTests()
        {
            _mockGameService = new Mock<IGameService>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_" + Guid.NewGuid().ToString())
                .Options;
    
            _context = new AppDbContext(options);
            _controller = new GameController(_mockGameService.Object, null, _context);
        }

        [Fact]
        public async Task GetGame_ExistingId_ReturnsGame()
        {
            // Arrange
            int gameId = 1;
            var expectedGame = new GameClass { Id = gameId };
            _mockGameService.Setup(s => s.GetGame(gameId))
                .ReturnsAsync(expectedGame);

            // Act
            var result = await _controller.GetGame(gameId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<GameClass>>(result);
            var returnValue = Assert.IsType<GameClass>(actionResult.Value);
            Assert.Equal(gameId, returnValue.Id);
        }

        [Fact]
        public async Task GetGame_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int gameId = 999;
            _mockGameService.Setup(s => s.GetGame(gameId))
                .ReturnsAsync((GameClass)null);

            // Act
            var result = await _controller.GetGame(gameId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<GameClass>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        
        [Fact]
        public async Task GetAllGames_ReturnsAllGames()
        {
            // Arrange
            var gameType = new GameTypeClass 
            { 
                Id = 1,
                GameTypeName = "Test Game Type"
            };
    
            var gameLevel = new GameLevelClass 
            { 
                Id = 1,
                GameLevelName = "Test Game Level"
            };

            await _context.GameType.AddAsync(gameType);
            await _context.GameLevel.AddAsync(gameLevel);
            await _context.SaveChangesAsync();

            var testGames = new List<GameClass>
            {
                new GameClass 
                { 
                    Id = 1,
                    Words = "test words 1",
                    Players = new List<PlayerClass> 
                    { 
                        new PlayerClass 
                        { 
                            Id = 1,
                            ConnectionGUID = Guid.NewGuid().ToString(),
                            NickName = "Player1"
                        } 
                    },
                    GameType = gameType,
                    GameLevel = gameLevel,
                    GameTypeId = gameType.Id,
                    GameLevelId = gameLevel.Id
                },
                new GameClass 
                { 
                    Id = 2,
                    Words = "test words 2",
                    Players = new List<PlayerClass> 
                    { 
                        new PlayerClass 
                        { 
                            Id = 2,
                            ConnectionGUID = Guid.NewGuid().ToString(),
                            NickName = "Player2"
                        } 
                    },
                    GameType = gameType,
                    GameLevel = gameLevel,
                    GameTypeId = gameType.Id,
                    GameLevelId = gameLevel.Id
                }
            };

            await _context.Games.AddRangeAsync(testGames);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAllGames();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GameClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameClass>>(okResult.Value);
            Assert.Equal(2, returnedGames.Count());
        }

        [Fact]
        public async Task GetAllGames_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange
            // Database starts empty, no need to add any data

            // Act
            var result = await _controller.GetAllGames();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GameClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedGames = Assert.IsAssignableFrom<IEnumerable<GameClass>>(okResult.Value);
            Assert.Empty(returnedGames);
        }
        
        [Fact]
        public async Task GetPlayersInGame_ExistingGame_ReturnsPlayers()
        {
            // Arrange
            var gameType = new GameTypeClass 
            { 
                Id = 1,
                GameTypeName = "Test Game Type"
            };
    
            var gameLevel = new GameLevelClass 
            { 
                Id = 1,
                GameLevelName = "Test Game Level"
            };

            await _context.GameType.AddAsync(gameType);
            await _context.GameLevel.AddAsync(gameLevel);
            await _context.SaveChangesAsync();

            var game = new GameClass 
            { 
                Id = 1,
                Words = "test words",
                Players = new List<PlayerClass> 
                { 
                    new PlayerClass 
                    { 
                        Id = 1,
                        ConnectionGUID = Guid.NewGuid().ToString(),
                        NickName = "Player1"
                    },
                    new PlayerClass 
                    { 
                        Id = 2,
                        ConnectionGUID = Guid.NewGuid().ToString(),
                        NickName = "Player2"
                    }
                },
                GameType = gameType,
                GameLevel = gameLevel,
                GameTypeId = gameType.Id,
                GameLevelId = gameLevel.Id
            };

            _mockGameService.Setup(s => s.GetGame(1))
                .ReturnsAsync(game);

            // Act
            var result = await _controller.GetPlayersInGame(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var players = Assert.IsAssignableFrom<IEnumerable<PlayerClass>>(okResult.Value);
            Assert.Equal(2, players.Count());
        }

        [Fact]
        public async Task GetPlayersInGame_NonExistingGame_ReturnsNotFound()
        {
            // Arrange
            _mockGameService.Setup(s => s.GetGame(999))
                .ReturnsAsync((GameClass)null);

            // Act
            var result = await _controller.GetPlayersInGame(999);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerClass>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
        
        [Fact]
        public void GetGameLevels_ReturnsAllLevels()
        {
            // Arrange
            var gameLevels = new List<GameLevelClass>
            {
                new GameLevelClass { Id = 1, GameLevelName = "Level 1" },
                new GameLevelClass { Id = 2, GameLevelName = "Level 2" }
            };

            _mockGameService.Setup(s => s.GameLevels)
                .Returns(gameLevels);

            // Act
            var result = _controller.GetGameLevels();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GameTypeClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedLevels = Assert.IsAssignableFrom<IEnumerable<GameLevelClass>>(okResult.Value);
            Assert.Equal(2, returnedLevels.Count());
            Assert.Contains(returnedLevels, l => l.GameLevelName == "Level 1");
            Assert.Contains(returnedLevels, l => l.GameLevelName == "Level 2");
        }

        [Fact]
        public void GetGameLevels_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var emptyLevels = new List<GameLevelClass>();
            _mockGameService.Setup(s => s.GameLevels)
                .Returns(emptyLevels);

            // Act
            var result = _controller.GetGameLevels();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GameTypeClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedLevels = Assert.IsAssignableFrom<IEnumerable<GameLevelClass>>(okResult.Value);
            Assert.Empty(returnedLevels);
        }
        
        [Fact]
        public void GetGameTypes_ReturnsAllTypes()
        {
            // Arrange
            var gameTypes = new List<GameTypeClass>
            {
                new GameTypeClass { Id = 1, GameTypeName = "Type 1" },
                new GameTypeClass { Id = 2, GameTypeName = "Type 2" }
            };

            _mockGameService.Setup(s => s.GameTypes)
                .Returns(gameTypes);

            // Act
            var result = _controller.GetGameTypes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GameLevelClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedTypes = Assert.IsAssignableFrom<IEnumerable<GameTypeClass>>(okResult.Value);
            Assert.Equal(2, returnedTypes.Count());
            Assert.Contains(returnedTypes, t => t.GameTypeName == "Type 1");
            Assert.Contains(returnedTypes, t => t.GameTypeName == "Type 2");
        }

        [Fact]
        public void GetGameTypes_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var emptyTypes = new List<GameTypeClass>();
            _mockGameService.Setup(s => s.GameTypes)
                .Returns(emptyTypes);

            // Act
            var result = _controller.GetGameTypes();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GameLevelClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedTypes = Assert.IsAssignableFrom<IEnumerable<GameTypeClass>>(okResult.Value);
            Assert.Empty(returnedTypes);
        }
        
        [Fact]
        public void GetGamePowers_ReturnsAllPowers()
        {
            // Arrange
            var powers = new List<PlayerPowerClass>
            {
                new PlayerPowerClass 
                { 
                    Id = 1, 
                    PlayerPowerName = "Power 1",
                    ImagePath = "path/to/image1",
                    PlayerPowerKey = "KEY1",
                    CooldownTime = 30,
                    IsTimedPower = true
                },
                new PlayerPowerClass 
                { 
                    Id = 2, 
                    PlayerPowerName = "Power 2",
                    ImagePath = "path/to/image2",
                    PlayerPowerKey = "KEY2",
                    CooldownTime = 60,
                    IsTimedPower = false
                }
            };

            _mockGameService.Setup(s => s.Powers)
                .Returns(powers);

            // Act
            var result = _controller.GetGamePowers();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerPowerClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPowers = Assert.IsAssignableFrom<IEnumerable<PlayerPowerClass>>(okResult.Value);
            Assert.Equal(2, returnedPowers.Count());
            Assert.Contains(returnedPowers, p => p.PlayerPowerName == "Power 1" && p.PlayerPowerKey == "KEY1");
            Assert.Contains(returnedPowers, p => p.PlayerPowerName == "Power 2" && p.PlayerPowerKey == "KEY2");
        }

        [Fact]
        public void GetGamePowers_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var emptyPowers = new List<PlayerPowerClass>();
            _mockGameService.Setup(s => s.Powers)
                .Returns(emptyPowers);

            // Act
            var result = _controller.GetGamePowers();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerPowerClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPowers = Assert.IsAssignableFrom<IEnumerable<PlayerPowerClass>>(okResult.Value);
            Assert.Empty(returnedPowers);
        }
        
        [Fact]
        public void GetWordStyles_WhenDatabaseFails_UsesGameServiceWordStyles()
        {
            // Arrange
            var wordStyles = new List<WordsStyleClass>
            {
                new WordsStyleClass 
                { 
                    Id = 1,
                    StyleName = "Style1",
                    fontFamily = "Arial",
                    fontWeight = "bold",
                    fontStyle = null        // This one won't be added because fontStyle is null
                },
                new WordsStyleClass 
                { 
                    Id = 2,
                    StyleName = "Style2",
                    fontFamily = "Times New Roman",
                    fontWeight = "normal",
                    fontStyle = "normal"    // This one will be added because fontStyle is not empty
                }
            };

            _mockGameService.Setup(s => s.WordStyles)
                .Returns(wordStyles);

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(decoratedWords);
            Assert.Single(decoratedWords);
            Assert.Contains(decoratedWords, w => w.Contains("font-weight:normal"));
        }

        [Fact]
        public void GetWordStyles_WithDatabaseFiller_ProcessesDataTableCorrectly()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("StyleName", typeof(string));
            dataTable.Columns.Add("fontStyle", typeof(string));
            dataTable.Columns.Add("fontWeight", typeof(string));
            dataTable.Columns.Add("fontFamily", typeof(string));

            dataTable.Rows.Add(1, "TestStyle", "italic", "bold", "Arial");
            dataTable.Rows.Add(2, "TestStyle2", "normal", "normal", "Times New Roman");

            // Test the Filler method directly
            var wordsStyle = new WordsStyleClass();
            var filledStyles = wordsStyle.Filler(dataTable);

            // Assert
            Assert.NotNull(filledStyles);
            Assert.Equal(2, filledStyles.Count);
            Assert.Contains(filledStyles, w => w.Id == 1 && w.StyleName == "TestStyle" && 
                                             w.fontStyle == "italic" && w.fontWeight == "bold" && 
                                             w.fontFamily == "Arial");
            Assert.Contains(filledStyles, w => w.Id == 2 && w.StyleName == "TestStyle2" && 
                                             w.fontStyle == "normal" && w.fontWeight == "normal" && 
                                             w.fontFamily == "Times New Roman");
        }
        
        [Fact]
        public void GetWordStyles_WhenDatabaseThrowsException_UsesGameServiceWordStyles()
        {
            // Arrange
            var wordStyles = new List<WordsStyleClass>
            {
                new WordsStyleClass 
                { 
                    Id = 1,
                    StyleName = "Style1",
                    fontWeight = "normal",
                    fontStyle = "normal"
                }
            };

            _mockGameService.Setup(s => s.WordStyles)
                .Returns(wordStyles);

            // The database operations will throw in the try block
            // and fall back to GameService

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(decoratedWords);
            Assert.Contains(decoratedWords, w => w.Contains("font-weight:normal"));
        }

        [Fact]
        public void GetWordStyles_WithNullProperties_HandlesNullsCorrectly()
        {
            // Arrange
            var wordStyles = new List<WordsStyleClass>
            {
                new WordsStyleClass 
                { 
                    Id = 1,
                    StyleName = "Style1",
                    fontFamily = null,      // Test null fontFamily
                    fontWeight = "normal",
                    fontStyle = "normal"
                }
            };

            _mockGameService.Setup(s => s.WordStyles)
                .Returns(wordStyles);

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(decoratedWords);
            Assert.Contains(decoratedWords, w => w.Contains("font-weight:normal"));
            Assert.DoesNotContain(decoratedWords, w => w.Contains("font-family"));
        }

        [Fact]
        public void GetWordStyles_WithEmptyProperties_HandlesEmptyStringsCorrectly()
        {
            // Arrange
            var wordStyles = new List<WordsStyleClass>
            {
                new WordsStyleClass 
                { 
                    Id = 1,
                    StyleName = "Style1",
                    fontFamily = "",        // Test empty fontFamily
                    fontWeight = "normal",
                    fontStyle = "normal"
                }
            };

            _mockGameService.Setup(s => s.WordStyles)
                .Returns(wordStyles);

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(decoratedWords);
            Assert.Contains(decoratedWords, w => w.Contains("font-weight:normal"));
            Assert.DoesNotContain(decoratedWords, w => w.Contains("font-family"));
        }

        [Fact]
        public void GetWordStyles_WithEmptyStyles_ReturnsEmptyList()
        {
            // Arrange
            var emptyStyles = new List<WordsStyleClass>();
            _mockGameService.Setup(s => s.WordStyles)
                .Returns(emptyStyles);

            var database = new DatabaseReceiver();
            database._results = null;

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.Empty(decoratedWords);
        }

        [Fact]
        public void GetWordStyles_WithAllDecorators_AppliesAllStyles()
        {
            // Arrange
            var wordStyles = new List<WordsStyleClass>
            {
                new WordsStyleClass 
                { 
                    Id = 1,
                    StyleName = "FullStyle",
                    fontFamily = "Arial",
                    fontWeight = "bold",
                    fontStyle = "italic"    // All decorators should be applied
                }
            };

            _mockGameService.Setup(s => s.WordStyles)
                .Returns(wordStyles);

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(decoratedWords);
            Assert.Single(decoratedWords);
            var decoratedWord = decoratedWords.First();
            Assert.Contains("font-weight:bold", decoratedWord);
        }

        [Fact]
        public void GetWordStyles_WithPartialDecorators_AppliesOnlySpecifiedStyles()
        {
            // Arrange
            var wordStyles = new List<WordsStyleClass>
            {
                new WordsStyleClass 
                { 
                    Id = 1,
                    StyleName = "PartialStyle",
                    fontFamily = "Arial",
                    fontWeight = "normal",
                    fontStyle = "italic"    // Should be applied
                }
            };

            _mockGameService.Setup(s => s.WordStyles)
                .Returns(wordStyles);

            // Act
            var result = _controller.GetWordStyles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<string>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var decoratedWords = Assert.IsAssignableFrom<IEnumerable<string>>(okResult.Value);
            Assert.NotEmpty(decoratedWords);
            Assert.Single(decoratedWords);
            var decoratedWord = decoratedWords.First();
            Assert.Contains("font-weight:normal", decoratedWord);
        }

        [Fact]
        public void GetPlayerGameResults_ReturnsAllResults()
        {
            // Arrange
            var gameResults = new List<PlayerGameResultTypeClass>
            {
                new PlayerGameResultTypeClass 
                { 
                    Id = 1,
                    Title = "Winner",
                    Text = "Congratulations!",
                    GifUrl = "winner.gif"
                },
                new PlayerGameResultTypeClass 
                { 
                    Id = 2,
                    Title = "Loser",
                    Text = "Better luck next time!",
                    GifUrl = "loser.gif"
                }
            };

            _mockGameService.Setup(s => s.PlayerGameResults)
                .Returns(gameResults);

            // Act
            var result = _controller.GetPlayerGameResults();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerPowerClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedResults = Assert.IsAssignableFrom<IEnumerable<PlayerGameResultTypeClass>>(okResult.Value);
            Assert.Equal(2, returnedResults.Count());
            Assert.Contains(returnedResults, r => r.Title == "Winner" && r.Text == "Congratulations!" && r.GifUrl == "winner.gif");
            Assert.Contains(returnedResults, r => r.Title == "Loser" && r.Text == "Better luck next time!" && r.GifUrl == "loser.gif");
        }

        [Fact]
        public void GetPlayerGameResults_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var emptyResults = new List<PlayerGameResultTypeClass>();
            _mockGameService.Setup(s => s.PlayerGameResults)
                .Returns(emptyResults);

            // Act
            var result = _controller.GetPlayerGameResults();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerPowerClass>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedResults = Assert.IsAssignableFrom<IEnumerable<PlayerGameResultTypeClass>>(okResult.Value);
            Assert.Empty(returnedResults);
        }
        
        [Fact]
        public async Task GetPlayerPowers_ReturnsPlayerPowers()
        {
            // Arrange
            int playerId = 1;
            var playerPowers = new List<PlayerPowerUseRelation>
            {
                new PlayerPowerUseRelation 
                { 
                    Id = 1,
                    ImagePath = "path/to/power1.png",
                    PlayerPowerKey = "POWER1",
                    IsUsed = false
                },
                new PlayerPowerUseRelation 
                { 
                    Id = 2,
                    ImagePath = "path/to/power2.png",
                    PlayerPowerKey = "POWER2",
                    IsUsed = true
                }
            };

            _mockGameService.Setup(s => s.GetPlayerPowers(playerId))
                .ReturnsAsync(playerPowers);

            // Act
            var result = await _controller.GetPlayerPowers(playerId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerPowerUseRelation>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPowers = Assert.IsAssignableFrom<IEnumerable<PlayerPowerUseRelation>>(okResult.Value);
            Assert.Equal(2, returnedPowers.Count());
            Assert.Contains(returnedPowers, p => 
                p.Id == 1 && 
                p.ImagePath == "path/to/power1.png" && 
                p.PlayerPowerKey == "POWER1" && 
                p.IsUsed == false);
            Assert.Contains(returnedPowers, p => 
                p.Id == 2 && 
                p.ImagePath == "path/to/power2.png" && 
                p.PlayerPowerKey == "POWER2" && 
                p.IsUsed == true);
        }

        [Fact]
        public async Task GetPlayerPowers_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            int playerId = 1;
            var emptyPowers = new List<PlayerPowerUseRelation>();

            _mockGameService.Setup(s => s.GetPlayerPowers(playerId))
                .ReturnsAsync(emptyPowers);

            // Act
            var result = await _controller.GetPlayerPowers(playerId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PlayerPowerUseRelation>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedPowers = Assert.IsAssignableFrom<IEnumerable<PlayerPowerUseRelation>>(okResult.Value);
            Assert.Empty(returnedPowers);
        }
    }
}