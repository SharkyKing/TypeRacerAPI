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
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;

namespace TypeRacerAPITest
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _mockGameService;
        private readonly GameController _controller;

        public GameControllerTests()
        {
            _mockGameService = new Mock<IGameService>();
            _controller = new GameController(_mockGameService.Object, null, null);
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
    }
}