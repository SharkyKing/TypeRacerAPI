using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Facade;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPITest.DesignPatternTests.Facade
{
    public class GameCreateFacadeTests
    {
        private readonly Mock<AppDbContext> _mockDbContext;
        private readonly Mock<IHubContext<GameHub>> _mockHubContext;
        private readonly Mock<IObserverController> _mockObserverController;
        private readonly Mock<IGameService> _mockGameService;
        private readonly GameCreateFacade _gameCreateFacade;
        public GameCreateFacadeTests()
        {
            // Setup DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            _mockDbContext = new Mock<AppDbContext>(options);

            // Mock other dependencies
            _mockHubContext = new Mock<IHubContext<GameHub>>();
            _mockObserverController = new Mock<IObserverController>();
            _mockGameService = new Mock<IGameService>();

            var wordsData = SeedData.WordsSeed.AsQueryable(); 
            var mockWordsDbSet = new Mock<DbSet<WordsClass>>();

            mockWordsDbSet.As<IQueryable<WordsClass>>().Setup(m => m.Provider).Returns(wordsData.Provider);
            mockWordsDbSet.As<IQueryable<WordsClass>>().Setup(m => m.Expression).Returns(wordsData.Expression);
            mockWordsDbSet.As<IQueryable<WordsClass>>().Setup(m => m.ElementType).Returns(wordsData.ElementType);
            mockWordsDbSet.As<IQueryable<WordsClass>>().Setup(m => m.GetEnumerator()).Returns(wordsData.GetEnumerator());

            _mockDbContext.Setup(db => db.Words).Returns(mockWordsDbSet.Object);

            // Initialize GameCreateFacade with mocks
            _gameCreateFacade = new GameCreateFacade(
                _mockDbContext.Object,
                _mockHubContext.Object,
                _mockObserverController.Object,
                _mockGameService.Object
            );
        }

        [Fact]
        public async Task Execute_ShouldReturnGame_WhenGameLevelAndTypeExist()
        {
            // Arrange
            string nickName = "TestPlayer";
            string connectionGUID = "GUID123";
            int activeGameType = 1;
            int activeGameLevel = 2;
            int gameId = 10;

            var mockGameLevel = new GameLevelClass { Id = activeGameLevel };
            var mockGameType = new GameTypeClass { Id = activeGameType };
            var mockGame = new GameClass { Id = gameId };

            _mockGameService.Setup(x => x.GameLevels).Returns(new[] { mockGameLevel }.AsEnumerable());
            _mockGameService.Setup(x => x.GameTypes).Returns(new[] { mockGameType }.AsEnumerable());
            _mockGameService
                .Setup(x => x.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()))
                .Returns(ValueTask.CompletedTask);

            _mockDbContext.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            // Act
            var result = await _gameCreateFacade.Execute(nickName, connectionGUID, activeGameType, activeGameLevel, gameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameId, result.Id);
            _mockGameService.Verify(x => x.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()), Times.Once);
        }
    }
}
