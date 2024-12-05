using Backend.BehavioralPatterns;
using Backend.Hubs;
using System.Net;

namespace Backend;

public class Startup
{
	public Startup() { }

	public WebApplication Application { get; private set; }
	public Task? RunningTask { get; private set; }

	public async Task StartAsync()
	{
		var builder = WebApplication.CreateBuilder();

		builder.WebHost.ConfigureKestrel((context, serverOptions) =>
		{
			serverOptions.Listen(IPAddress.Loopback, 5270);
		});

		ConfigureServices(builder.Services);

		Application = builder.Build();
		ConfigureMiddlewares(Application);

		RunningTask = Application.RunAsync();
		await RunningTask;
	}

	private static void ConfigureServices(IServiceCollection services)
	{
		services.AddSignalR();
		services.AddSingleton<PlayerRepository>();
		services.AddSingleton<EnemyRepository>();
		services.AddSingleton<GameUpdater>();
		services.AddSingleton<PlayerController>();
	}

	private static void ConfigureMiddlewares(WebApplication app)
	{
		app.MapHub<MainHub>("/hubs:main");
	}
}