using Microsoft.AspNetCore.SignalR.Client;

namespace Client.HubClients;

public class MainHubClient : IHubClient
{
	public string Route { get; } = "/hubs:main";
	public HubConnection Connection { get; private set; }

	private MainHubClient()
	{
		Connection = new HubConnectionBuilder()
			.WithUrl(new Uri($"{Globals.BACKEND_BASE_URL}{Route}"))
			.WithAutomaticReconnect()
			.Build();
	}

	private static MainHubClient? _client;
	public static async Task<MainHubClient> GetClientAsync(CancellationToken cancellationToken = default)
	{
		if (_client == null)
		{
			_client = new MainHubClient();
			await _client.Connection.StartAsync(cancellationToken);
		}

		return _client;
	}
}