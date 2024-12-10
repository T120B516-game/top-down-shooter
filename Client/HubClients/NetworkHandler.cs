using Client.Observer;
using Microsoft.AspNetCore.SignalR.Client;

namespace Client.HubClients;

public interface INetworkHandler
{
    Task SendMovementAsync(Direction direction, int playerId);
    Task SendTeleportAsync(int x, int y, int playerId);  // Sends teleport coordinates
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

    public async Task SendTeleportAsync(int x, int y, int playerId)
    {
        await _hubConnection.SendAsync("teleportPlayer", x, y, playerId);
    }
}