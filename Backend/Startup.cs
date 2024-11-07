using Backend.BehavioralPatterns;
using Backend.Hubs;
using System.Net;

namespace Backend;

public class Startup
{
	public Startup() { }

	public async Task StartAsync()
	{
		var builder = WebApplication.CreateBuilder();

		builder.WebHost.ConfigureKestrel((context, serverOptions) =>
		{
			serverOptions.Listen(IPAddress.Loopback, 5270);
		});

		ConfigureServices(builder.Services);

		var app = builder.Build();
		ConfigureMiddlewares(app);

		await app.RunAsync();
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