using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace Backend.Hubs;

public class MainHub : Hub
{
	private readonly PlayerRepository _playerRepository;
	private readonly GameUpdater _gameUpdater;

	public MainHub(
		PlayerRepository playerRepository, 
		GameUpdater gameUpdater)
	{
		_playerRepository = playerRepository;
		_gameUpdater = gameUpdater;
	}

	private static ConcurrentDictionary<string, string> _connectedUsers = [];

	public override async Task OnConnectedAsync()
	{
		_connectedUsers.TryAdd(Context.ConnectionId, Context.ConnectionId);

		if (_connectedUsers.Count == 2)
		{
			_gameUpdater.Start(Clients);
		}
		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception exception)
	{
		_connectedUsers.TryRemove(Context.ConnectionId, out _);
		await Clients.All.SendAsync("ReceiveConnectedUsersCount", _connectedUsers.Count);

		await base.OnDisconnectedAsync(exception);
	}

	[HubMethodName("CreatePlayer")]
	public async Task CreatePlayerAsync()
	{
		var player = await _playerRepository.CreateAsync();
		await Clients.Caller.SendAsync("ReceivePersonalId", player.Id);
	}

	[HubMethodName("RequestConnectedUsersCount")]
	public async Task RequestConnectedUsersCountAsync()
	{
		await Clients.Caller.SendAsync("ReceiveConnectedUsersCount", _connectedUsers.Count);
	}

	/// <summary>
	/// Updates players position acording to the received direction code
	/// </summary>
	/// <param name="direction">Integer which references direction (1 - up, 2 - right, 3 - down, 4 - left)</param>
	/// <param name="playerId">The id of the player, whose coordinates need to be changed</param>
	[HubMethodName("MovePlayer")]
	public async Task MovePlayerAsync(int direction, int playerId)
	{
		var player = await _playerRepository.GetAsync(playerId);
		if(player == null)
		{
			return;
		}

		switch (direction)
		{
			case 1:
				player.Y -= player.Speed;
				player.Image = "PlayerUp";
				break;
			case 2:
				player.X += player.Speed;
				player.Image = "PlayerRight";
				break;
			case 3:
				player.Y += player.Speed;
				player.Image = "PlayerDown";
				break;
			case 4:
				player.X -= player.Speed;
				player.Image = "PlayerLeft";
				break;
		}
	}
}
