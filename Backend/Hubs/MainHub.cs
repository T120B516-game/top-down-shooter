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
		await Clients.All.SendAsync("ReceiveConnectedUsersCount", _connectedUsers.Count);

		await base.OnConnectedAsync();
	}

	public override async Task OnDisconnectedAsync(Exception exception)
	{
		_connectedUsers.TryRemove(Context.ConnectionId, out _);
		await Clients.All.SendAsync("ReceiveConnectedUsersCount", _connectedUsers.Count);

		await base.OnDisconnectedAsync(exception);
	}

	[HubMethodName("RequestConnectedUsersCount")]
	public async Task RequestConnectedUsersCountAsync()
	{
		await Clients.Caller.SendAsync("ReceiveConnectedUsersCount", _connectedUsers.Count);
	}
}
