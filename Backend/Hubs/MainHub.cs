using Backend.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Linq;

namespace Backend.Hubs;

public class MainHub : Hub
{
	private static ConcurrentDictionary<string, string> _connectedUsers = [];

	public override async Task OnConnectedAsync()
	{
		_connectedUsers.TryAdd(Context.ConnectionId, Context.ConnectionId);

		if (_connectedUsers.Count == 2)
		{
			GameUpdater.StartGameUpdater(Clients);
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
		Player player = new Player(GameData.NextID);
		GameData.Players.Add(player);
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
		var player = GameData.Players.Where(p => p.Id == playerId).FirstOrDefault();
        if (player is not null)
        {
            switch (direction)
			{
				case 1:
					player.Vertical -= GameData.Speed;
					player.Image = "PlayerUp";
				break;
				case 2:
					player.Horizontal += GameData.Speed;
					player.Image = "PlayerRight";
					break;
				case 3:
					player.Vertical += GameData.Speed;
					player.Image = "PlayerDown";
					break;
				case 4:
					player.Horizontal -= GameData.Speed;
					player.Image = "PlayerLeft";
					break;
			}
        }
    }
}
