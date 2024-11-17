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
        private readonly AppDbContext _dbContext;
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

            _dbContext = new AppDbContext(options);

            // Mock other dependencies
            _mockHubContext = new Mock<IHubContext<GameHub>>();
            _mockObserverController = new Mock<IObserverController>();
            _mockGameService = new Mock<IGameService>();

            _dbContext.Words.AddRange(SeedData.WordsSeed); // Use real data from SeedData
            _dbContext.SaveChanges();

            _gameCreateFacade = new GameCreateFacade(
                _dbContext,
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

            var mockGameLevel = new GameLevelClass { Id = activeGameLevel };
            var mockGameType = new GameTypeClass { Id = activeGameType };

            _mockGameService.Setup(x => x.GameLevels).Returns(new[] { mockGameLevel }.AsEnumerable());
            _mockGameService.Setup(x => x.GameTypes).Returns(new[] { mockGameType }.AsEnumerable());
            _mockGameService
                .Setup(x => x.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()))
                .Returns(ValueTask.CompletedTask);

            _dbContext.SaveChangesAsync().Wait();

            // Act
            var result = await _gameCreateFacade.Execute(nickName, connectionGUID, activeGameType, activeGameLevel, 0);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            _mockGameService.Verify(x => x.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()), Times.Once);
        }
    }
}
