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
using TypeRacerAPI.Services.Interface;
using System.Linq.Expressions;
using static TypeRacerAPI.EnumClass;

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
			var game = new GameClass();
			var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict, null);

			// Act
			await _gameTimerService.StartInitiatingGame(game, serviceProvider.Object);

			// Assert
			Assert.NotNull(game);
			Assert.True(game.IsOpen);
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
			var serviceProvider = new Mock<IServiceProvider>();
			var player = new PlayerClass { Id = playerId };
			_mockGameService.Setup(s => s.GetPlayer(playerId.Value)).ReturnsAsync(player);

			// Act
			await _gameTimerService.PowerCoolDownTimer(playerId, connectionId, serviceProvider.Object);

			// Assert
			var retrievedPlayer = await _mockGameService.Object.GetPlayer(playerId.Value);
			Assert.NotNull(retrievedPlayer);
			// Add more assertions based on the expected state of the player after cooldown
		}
	}
}
