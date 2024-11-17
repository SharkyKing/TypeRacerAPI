using Moq;
using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.DesignPatterns.Facade;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPITest
{
    public class GameHubTests
    {
        private readonly GameHub _gameHub;
        private readonly DependencyMocks _mocks;

        public GameHubTests()
        {
            _mocks = new DependencyMocks();
            _gameHub = new GameHub(
                _mocks.AppDbContextMock.Object,
                _mocks.HubContextMock.Object,
                _mocks.GameTimerServiceMock.Object,
                _mocks.GameServiceMock.Object,
                _mocks.ObserverControllerMock.Object,
                _mocks.ServiceProviderMock.Object
            );
        }

        [Fact]
        public async Task CreateGame_ShouldCallExpectedMethods()
        {
            // Arrange
            string nickName = "TestUser";
            int activeGameType = 1;
            int activeGameLevel = 2;
            string connectionGUID = "test-guid";

            var mockGame = new GameClass { Id = 123 };
            var gameCreateFacadeMock = new Mock<GameCreateFacade>(
                _mocks.AppDbContextMock.Object,
                _mocks.HubContextMock.Object,
                _mocks.ObserverControllerMock.Object,
                _mocks.GameServiceMock.Object
            );

            gameCreateFacadeMock
                .Setup(f => f.Execute(nickName, connectionGUID, activeGameType, activeGameLevel, 0))
                .ReturnsAsync(mockGame);

            // Act
            await _gameHub.CreateGame(nickName, activeGameType, activeGameLevel, connectionGUID);

            // Assert
            _mocks.ObserverControllerMock.Verify(o => o.Attach(It.IsAny<GameObserver>()), Times.Once);
            _mocks.GroupManagerMock.Verify(g => g.AddToGroupAsync(It.IsAny<string>(), "123", default), Times.Once);
        }
    }

}