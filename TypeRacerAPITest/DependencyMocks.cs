namespace TypeRacerAPITest;

using Moq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TypeRacerAPI.Data;
using TypeRacerAPI.DesignPatterns.Observer;
using TypeRacerAPI.DesignPatterns.Singleton.GameService;
using TypeRacerAPI.Hubs;
using TypeRacerAPI.Services;

public class DependencyMocks
{
    public Mock<AppDbContext> AppDbContextMock { get; private set; }
    public Mock<IHubContext<GameHub>> HubContextMock { get; private set; }
    public Mock<GameTimerService> GameTimerServiceMock { get; private set; }
    public Mock<GameService> GameServiceMock { get; private set; }
    public Mock<ObserverController> ObserverControllerMock { get; private set; }
    public Mock<IServiceProvider> ServiceProviderMock { get; private set; }
    public Mock<IHubClients> HubClientsMock { get; private set; }
    public Mock<IGroupManager> GroupManagerMock { get; private set; }

    public DependencyMocks()
    {
        // Setup AppDbContext Mock
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;
        AppDbContextMock = new Mock<AppDbContext>(options);

        // Setup IHubContext<GameHub> Mock
        HubContextMock = new Mock<IHubContext<GameHub>>();
        HubClientsMock = new Mock<IHubClients>();
        GroupManagerMock = new Mock<IGroupManager>();
        HubContextMock.Setup(h => h.Clients).Returns(HubClientsMock.Object);
        HubContextMock.Setup(h => h.Groups).Returns(GroupManagerMock.Object);

        // Setup ObserverController Mock
        ObserverControllerMock = new Mock<ObserverController>();

        // Setup GameService Mock
        ServiceProviderMock = new Mock<IServiceProvider>();
        GameServiceMock = new Mock<GameService>(ServiceProviderMock.Object);

        // Setup GameTimerService Mock
        GameTimerServiceMock = new Mock<GameTimerService>(
            AppDbContextMock.Object,          // Use the existing AppDbContextMock
            HubContextMock.Object,            // Use the existing HubContextMock
            GameServiceMock.Object,           // Use the existing GameServiceMock
            ObserverControllerMock.Object     // Use the existing ObserverControllerMock
        );

        // Setup IServiceProvider Mock
        ServiceProviderMock.Setup(sp => sp.GetService(typeof(AppDbContext)))
            .Returns(AppDbContextMock.Object);
        ServiceProviderMock.Setup(sp => sp.GetService(typeof(IHubContext<GameHub>)))
            .Returns(HubContextMock.Object);
        ServiceProviderMock.Setup(sp => sp.GetService(typeof(GameTimerService)))
            .Returns(GameTimerServiceMock.Object);
        ServiceProviderMock.Setup(sp => sp.GetService(typeof(GameService)))
            .Returns(GameServiceMock.Object);
        ServiceProviderMock.Setup(sp => sp.GetService(typeof(ObserverController)))
            .Returns(ObserverControllerMock.Object);
    }
}

