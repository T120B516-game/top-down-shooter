using Backend.Hubs;

namespace Backend;

public class Startup
{
	public Startup() { }

	public async Task StartAsync()
	{
		var builder = WebApplication.CreateBuilder();
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
	}

	private static void ConfigureMiddlewares(WebApplication app)
	{
		app.MapHub<MainHub>("/hubs:main");
	}
}