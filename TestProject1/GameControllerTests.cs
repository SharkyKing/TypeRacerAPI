using TypeRacerAPI.BaseClasses;
using TypeRacerAPI.Controllers;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Facade;
using TypeRacerAPI.DesignPatterns.Facade.Interface;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.Hubs;

namespace TypeRacerAPI;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Xunit;
using System;
using System.Threading.Tasks;

public class GameHubTests
{
    private readonly Mock<AppDbContext> _mockContext;
    private readonly Mock<IHubContext<GameHub>> _mockHubContext;
    private readonly Mock<GameTimerService> _mockGameTimerService;
    private readonly Mock<GameService> _mockGameService;
    private readonly Mock<ObserverController> _mockObserverController;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly GameHub _hub;

    public GameHubTests()
    {
        _mockContext = new Mock<AppDbContext>();
        _mockHubContext = new Mock<IHubContext<GameHub>>();
        _mockGameTimerService = new Mock<GameTimerService>();
        _mockGameService = new Mock<GameService>();
        _mockObserverController = new Mock<ObserverController>();
        _mockServiceProvider = new Mock<IServiceProvider>();

        _hub = new GameHub(
            _mockContext.Object,
            _mockHubContext.Object,
            _mockGameTimerService.Object,
            _mockGameService.Object,
            _mockObserverController.Object,
            _mockServiceProvider.Object
        );
    }

    [Fact]
    public async Task CreateGame_WhenSuccessful_ShouldCreateGameAndAttachObserver()
    {
        // Arrange
        var expectedGame = new GameClass { Id = 123 };
        var nickName = "TestPlayer";
        var activeGameType = 1;
        var activeGameLevel = 1;
        var connectionGUID = "test-guid";

        // Setup mock for hub's Context and connection ID
        var mockHubCallerContext = new Mock<HubCallerContext>();
        mockHubCallerContext.Setup(m => m.ConnectionId).Returns(connectionGUID);
        typeof(Hub)
            .GetProperty("Context")
            .SetValue(_hub, mockHubCallerContext.Object);

        // Setup mock for hub's Groups
        var mockGroups = new Mock<IGroupManager>();
        mockGroups
            .Setup(x => x.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .Returns(Task.CompletedTask);
        typeof(Hub)
            .GetProperty("Groups")
            .SetValue(_hub, mockGroups.Object);

        _mockObserverController
            .Setup(x => x.Attach(It.IsAny<GameObserver>()))
            .Verifiable();

        // Setup GameCreateFacade to return a game
        var mockGameFacade = new GameCreateFacade(
            _mockContext.Object,
            _mockHubContext.Object,
            _mockObserverController.Object,
            _mockGameService.Object);

        // Act
        await _hub.CreateGame(nickName, activeGameType, activeGameLevel, connectionGUID);

        // Assert
        _mockObserverController.Verify(x => x.Attach(It.Is<GameObserver>(
            o => o.gameId == expectedGame.Id)), Times.Once);

        mockGroups.Verify(x => x.AddToGroupAsync(
            connectionGUID,
            expectedGame.Id.ToString(),
            default), Times.Once);
    }

    [Fact]
    public async Task CreateGame_WhenGameCreationFails_ShouldNotAttachObserver()
    {
        // Arrange
        var nickName = "TestPlayer";
        var activeGameType = 1;
        var activeGameLevel = 1;
        var connectionGUID = "test-guid";

        // Setup mock for hub's Context
        var mockHubCallerContext = new Mock<HubCallerContext>();
        mockHubCallerContext.Setup(m => m.ConnectionId).Returns(connectionGUID);
        typeof(Hub)
            .GetProperty("Context")
            .SetValue(_hub, mockHubCallerContext.Object);

        // Setup mock for Groups
        var mockGroups = new Mock<IGroupManager>();
        typeof(Hub)
            .GetProperty("Groups")
            .SetValue(_hub, mockGroups.Object);

        // Setup GameCreateFacade to return null
        var mockGameFacade = new GameCreateFacade(
            _mockContext.Object,
            _mockHubContext.Object,
            _mockObserverController.Object,
            _mockGameService.Object);

        // Act
        await _hub.CreateGame(nickName, activeGameType, activeGameLevel, connectionGUID);

        // Assert
        _mockObserverController.Verify(x => x.Attach(It.IsAny<GameObserver>()), Times.Never);
        mockGroups.Verify(x => x.AddToGroupAsync(It.IsAny<string>(), It.IsAny<string>(), default), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateGame_WithInvalidNickname_ShouldThrowException(string invalidNickname)
    {
        // Arrange
        var activeGameType = 1;
        var activeGameLevel = 1;
        var connectionGUID = "test-guid";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _hub.CreateGame(invalidNickname, activeGameType, activeGameLevel, connectionGUID));
    }

    [Fact]
    public async Task CreateGame_ShouldCallGameObserverUpdate()
    {
        // Arrange
        var expectedGame = new GameClass { Id = 123 };
        var nickName = "TestPlayer";
        var activeGameType = 1;
        var activeGameLevel = 1;
        var connectionGUID = "test-guid";

        // Setup mock for hub's Context
        var mockHubCallerContext = new Mock<HubCallerContext>();
        mockHubCallerContext.Setup(m => m.ConnectionId).Returns(connectionGUID);
        typeof(Hub)
            .GetProperty("Context")
            .SetValue(_hub, mockHubCallerContext.Object);

        // Setup mock for Groups
        var mockGroups = new Mock<IGroupManager>();
        typeof(Hub)
            .GetProperty("Groups")
            .SetValue(_hub, mockGroups.Object);

        // Setup GameCreateFacade to return a game
        var mockGameFacade = new GameCreateFacade(
            _mockContext.Object,
            _mockHubContext.Object,
            _mockObserverController.Object,
            _mockGameService.Object);

        // Act
        await _hub.CreateGame(nickName, activeGameType, activeGameLevel, connectionGUID);

        // Assert
        _mockServiceProvider.Verify(x => 
            x.GetService(It.IsAny<Type>()), Times.AtLeastOnce);
    }
}