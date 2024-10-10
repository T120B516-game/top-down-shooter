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
    private bool _shouldUpdateObstacles = true;

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

            if (_clients != null)
            {
                if (_shouldUpdateObstacles) // Only update if there are changes
                {
                    var obstacleData = JsonConvert.SerializeObject(ObstacleRepository.Obstacles);
                    await _clients.All.SendAsync("ReceiveObstaclesUpdate", obstacleData);
                    _shouldUpdateObstacles = false; // Reset the flag
                }

                await _clients.All.SendAsync("ReceiveGameUpdate", playersJson);
            }
        }
    }
    public void MarkObstaclesForUpdate() // Call this method when obstacles change
    {
        _shouldUpdateObstacles = true;
    }
}
