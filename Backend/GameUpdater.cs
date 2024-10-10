using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Backend;

/// <summary>
/// Main update cycle of the game
/// </summary>
public class GameUpdater
{
	private readonly PlayerRepository _playerRepository;

	private IHubCallerClients? _clients = null;
	private Task _broadcastingTask;

	public GameUpdater(PlayerRepository playerRepository)
	{
		_playerRepository = playerRepository;
	}

	/// <summary>
	/// Once this function is called, BroadcastGameUpdate starts getting invoked repeatedly (every _updateInterval), broadcasting the game data to all clients
	/// </summary>
	public void Start(IHubCallerClients hubContext)
	{
		if (_clients is not null) return;
		_clients = hubContext;

		_broadcastingTask = LoopBroadcastAsync();
	}

	private async Task LoopBroadcastAsync()
	{
		var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(20));

		while (await timer.WaitForNextTickAsync())
		{
			var players = await _playerRepository.ListAsync();
			var playersJson = JsonConvert.SerializeObject(players);

			await _clients.All.SendAsync("ReceiveGameUpdate", playersJson);
		}
	}
}
