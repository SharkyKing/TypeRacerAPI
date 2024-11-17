using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Facade;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.DesignPatterns.Observer.Interface;
using TypeRacerAPI.DesignPatterns.Factory.Player.Enum;
using TypeRacerAPI.DesignPatterns.Factory.Player;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TypeRacerAPITest.DesignPatternTests.Facade
{
    public class GameJoinFacadeTests
    {
        private readonly AppDbContext _dbContext;
        private readonly Mock<IHubContext<GameHub>> _mockHubContext;
        private readonly Mock<IObserverController> _mockObserverController;
        private readonly Mock<IGameService> _mockGameService;
        private readonly GameJoinFacade _gameJoinFacade;

        public GameJoinFacadeTests()
        {
            // Setup DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _mockHubContext = new Mock<IHubContext<GameHub>>();
            _mockObserverController = new Mock<IObserverController>();
            _mockGameService = new Mock<IGameService>();

            _gameJoinFacade = new GameJoinFacade(
                _dbContext,
                _mockHubContext.Object,
                _mockObserverController.Object,
                _mockGameService.Object
            );
        }

        [Fact]
        public async Task Execute_ShouldReturnGame_WhenGameExists()
        {
            // Arrange
            string nickName = "TestPlayer";
            string socketId = "Socket123";
            int gameId = 1;
            int activeGameType = 1;
            int activeGameLevel = 2;

            var mockGame = new GameClass { Id = gameId };

            // Setup mock to return a game when GetGame is called
            _mockGameService.Setup(service => service.GetGame(gameId))
                            .ReturnsAsync(mockGame);

            _mockGameService.Setup(service => service.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()))
                            .Returns(ValueTask.CompletedTask);

            // Act
            var result = await _gameJoinFacade.Execute(nickName, socketId, activeGameType, activeGameLevel, gameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameId, result.Id);
            _mockGameService.Verify(service => service.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()), Times.Once);
        }

        [Fact]
        public async Task Execute_ShouldReturnNull_WhenGameDoesNotExist()
        {
            // Arrange
            string nickName = "TestPlayer";
            string socketId = "Socket123";
            int gameId = 999; // Non-existent game ID
            int activeGameType = 1;
            int activeGameLevel = 2;

            // Setup mock to return null when GetGame is called for a non-existent game
            _mockGameService.Setup(service => service.GetGame(gameId))
                            .ReturnsAsync((GameClass)null);

            // Act
            var result = await _gameJoinFacade.Execute(nickName, socketId, activeGameType, activeGameLevel, gameId);

            // Assert
            Assert.Null(result);
            _mockGameService.Verify(service => service.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()), Times.Never);
        }

        [Fact]
        public async Task Execute_ShouldCallAddPlayerAsync_WhenGameExists()
        {
            // Arrange
            string nickName = "TestPlayer";
            string socketId = "Socket123";
            int gameId = 1;
            int activeGameType = 1;
            int activeGameLevel = 2;

            var mockGame = new GameClass { Id = gameId };

            // Setup mock to return a game when GetGame is called
            _mockGameService.Setup(service => service.GetGame(gameId))
                            .ReturnsAsync(mockGame);

            _mockGameService.Setup(service => service.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()))
                            .Returns(ValueTask.CompletedTask);

            // Act
            var result = await _gameJoinFacade.Execute(nickName, socketId, activeGameType, activeGameLevel, gameId);

            // Assert
            _mockGameService.Verify(service => service.AddPlayerAsync(It.IsAny<GameClass>(), It.IsAny<PlayerClass>()), Times.Once);
        }
    }
}
