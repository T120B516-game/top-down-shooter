using Microsoft.AspNetCore.SignalR.Client;

namespace Client.HubClients;

public interface IHubClient
{
	string Route { get; }
	HubConnection Connection { get; }
}
