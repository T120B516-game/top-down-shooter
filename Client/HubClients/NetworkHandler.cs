using Client.Observer;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.HubClients;

public interface INetworkHandler
{
	Task SendMovementAsync(Direction direction, int playerId);
}

public class NetworkHandler : INetworkHandler
{
	private readonly HubConnection _hubConnection;

	public NetworkHandler(HubConnection hubConnection)
	{
		_hubConnection = hubConnection;
	}

	public async Task SendMovementAsync(Direction direction, int playerId)
	{
		await _hubConnection.SendAsync("movePlayer", (int)direction, Globals.PersonalID);
	}
}