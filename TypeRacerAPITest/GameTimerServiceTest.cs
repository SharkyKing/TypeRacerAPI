using Moq;
using Xunit;
using TypeRacerAPI.Services;
using TypeRacerAPI.Data;
using TypeRacerAPI.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using Microsoft.EntityFrameworkCore;
using TypeRacerAPI.BaseClasses;
using System.Linq.Expressions;
using static TypeRacerAPI.EnumClass;
using TypeRacerAPI.DesignPatterns.Observer.Interface;

namespace TypeRacerAPITest
{
    public class GameTimerServiceTest
	{
		private readonly Mock<AppDbContext> _mockContext;
		private readonly Mock<IHubContext<GameHub>> _mockHubContext;
		private readonly Mock<IGameService> _mockGameService;
		private readonly Mock<IObserverController> _mockObserverController;
		private readonly GameTimerService _gameTimerService;

		public GameTimerServiceTest()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>().Options;
			_mockContext = new Mock<AppDbContext>(options);
			_mockHubContext = new Mock<IHubContext<GameHub>>();
			_mockGameService = new Mock<IGameService>();
			_mockObserverController = new Mock<IObserverController>();

			_gameTimerService = new GameTimerService(
				_mockContext.Object,
				_mockHubContext.Object,
				_mockGameService.Object,
				_mockObserverController.Object
			);
		}


		[Fact]
		public async Task StartInitiatingGame_ShouldStartGameSuccessfully()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			await using var dbContext = new AppDbContext(options);

			var game = new GameClass
			{
				Id = 1,
				IsOver = false,
				StartTime = 0,
				Words = "Sample words"
			};

			dbContext.Add(game);
			await dbContext.SaveChangesAsync();  // Save changes to the database

			var serviceProviderMock = new Mock<IServiceProvider>();
			var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
			var serviceScopeMock = new Mock<IServiceScope>();

			// Mock the ServiceScopeFactory and ServiceScope to return the appropriate mocked service provider
			serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceScopeMock.Object);
			serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
				.Returns(serviceScopeFactoryMock.Object);

			// Mock DbContext
			serviceProviderMock.Setup(sp => sp.GetService(typeof(AppDbContext)))
				.Returns(dbContext);

			// Mock IHubContext<GameHub> and related components
			var mockHubContext = new Mock<IHubContext<GameHub>>();
			var mockClients = new Mock<IHubClients>();
			var mockGroupClient = new Mock<IClientProxy>();

			mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(mockGroupClient.Object);
			mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

			serviceProviderMock.Setup(sp => sp.GetService(typeof(IHubContext<GameHub>)))
				.Returns(mockHubContext.Object);

			// Mocking IObserverController and Notify method
			var mockObserverController = new Mock<IObserverController>();
			mockObserverController.Setup(o => o.Notify(It.IsAny<IServiceProvider>()))
				.Returns(ValueTask.CompletedTask);  // Mock Notify to return a completed task

			// Set up the service provider mock to return the mocked observer controller
			serviceProviderMock.Setup(sp => sp.GetService(typeof(IObserverController)))
				.Returns(mockObserverController.Object);

			// Make sure the service scope returns the correct service provider
			serviceScopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);

			// Act
			await _gameTimerService.StartInitiatingGame(game, serviceProviderMock.Object);

			// Assert
			Assert.NotNull(game);
			Assert.False(game.IsOpen, "Game should be marked as open after initiation.");

		}



		[Fact]
		public async Task StartGameTimer_ShouldStartTimerSuccessfully()
		{
			// Arrange
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			await using var dbContext = new AppDbContext(options);
			dbContext.Games.Add(new GameClass
			{
				Id = 1,
				IsOver = false,
				StartTime = 0,
				Words = "Sample words"
			});
			await dbContext.SaveChangesAsync();

			// Mock HubContext and Clients
			var mockHubContext = new Mock<IHubContext<GameHub>>();
			var mockClients = new Mock<IHubClients>();
			var mockGroupClient = new Mock<IClientProxy>();

			// Setup mock for Clients.Group
			mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(mockGroupClient.Object);
			mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

			// Mock ServiceProvider and related dependencies
			var serviceProviderMock = new Mock<IServiceProvider>();
			var serviceScopeMock = new Mock<IServiceScope>();
			var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

			serviceScopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);
			serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceScopeMock.Object);

			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
				.Returns(serviceScopeFactoryMock.Object);
			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(AppDbContext)))
				.Returns(dbContext);
			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IHubContext<GameHub>)))
				.Returns(mockHubContext.Object);

			// Create a mock IServiceScopeFactory registration
			var services = new ServiceCollection();
			services.AddSingleton(serviceScopeFactoryMock.Object);
			var builtProvider = services.BuildServiceProvider();

			// Inject the new service provider into the mock IServiceProvider
			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IServiceProvider)))
				.Returns(builtProvider);

			int gameId = 1;

			// Act
			await _gameTimerService.StartGameTimer(gameId, serviceProviderMock.Object);

			// Assert
			var retrievedGame = await dbContext.Games.FindAsync(gameId);
			Assert.NotNull(retrievedGame);
			Assert.True(retrievedGame.StartTime > 0, "StartTime should be greater than 0 after starting the timer.");
		}


		[Fact]
		public async Task PowerCoolDownTimer_ShouldStartCooldownSuccessfully()
		{
			// Arrange
			int? playerId = 1;
			string connectionId = "test-connection-id";

			// Mock an in-memory database for AppDbContext
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;

			await using var dbContext = new AppDbContext(options);

			// Adding a player to the in-memory database with all required properties
			var player = new PlayerClass
			{
				Id = playerId.Value,
				ConnectionGUID = Guid.NewGuid().ToString(),  // Set a unique GUID for ConnectionGUID
				NickName = "TestPlayer"                      // Set a value for NickName
			};

			dbContext.Players.Add(player);
			await dbContext.SaveChangesAsync();

			// Mock HubContext and Clients
			var mockHubContext = new Mock<IHubContext<GameHub>>();
			var mockClients = new Mock<IHubClients>();
			var mockGroupClient = new Mock<IClientProxy>();

			// Setup mock for Clients.Group
			mockClients.Setup(c => c.Group(It.IsAny<string>())).Returns(mockGroupClient.Object);
			mockHubContext.Setup(hub => hub.Clients).Returns(mockClients.Object);

			// Mock the behavior of SendAsync
			mockGroupClient.Setup(client => client.SendCoreAsync(It.IsAny<string>(), It.IsAny<object[]>(), It.IsAny<CancellationToken>()))
				.Returns(Task.CompletedTask); // Simulating the SendAsync method completing successfully

			// Mock ServiceProvider and related dependencies
			var serviceProviderMock = new Mock<IServiceProvider>();
			var serviceScopeMock = new Mock<IServiceScope>();
			var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();

			serviceScopeMock.Setup(s => s.ServiceProvider).Returns(serviceProviderMock.Object);
			serviceScopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceScopeMock.Object);

			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
				.Returns(serviceScopeFactoryMock.Object);
			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(AppDbContext)))
				.Returns(dbContext);
			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IHubContext<GameHub>)))
				.Returns(mockHubContext.Object);

			// Mock GameService
			var mockGameService = new Mock<IGameService>();
			mockGameService.Setup(s => s.GetPlayer(playerId.Value)).ReturnsAsync(player);

			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IGameService)))
				.Returns(mockGameService.Object);

			// Create a mock IServiceScopeFactory registration
			var services = new ServiceCollection();
			services.AddSingleton(serviceScopeFactoryMock.Object);
			var builtProvider = services.BuildServiceProvider();

			// Inject the new service provider into the mock IServiceProvider
			serviceProviderMock
				.Setup(sp => sp.GetService(typeof(IServiceProvider)))
				.Returns(builtProvider);

			// Act
			await _gameTimerService.PowerCoolDownTimer(playerId, connectionId, serviceProviderMock.Object);

			// Assert
			var retrievedPlayer = await dbContext.Players.FindAsync(playerId.Value);
			Assert.NotNull(retrievedPlayer);
			Assert.True(retrievedPlayer.InputEnabled);  // Assuming the cooldown method enables/disables input
}





	}
}
