using Microsoft.AspNetCore.SignalR.Client;

namespace Client;

public interface ISendable
{
	Task SendAsync(HubConnection connection);
}
