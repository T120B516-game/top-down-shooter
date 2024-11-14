using Client.HubClients;
using Client.Observer;
using System.Collections.Immutable;

namespace Tests;

public class MockNetworkHandler : INetworkHandler
{
	private readonly List<Direction> _sentMovements = [];

	public ImmutableList<Direction> GetSentMovements() =>
		_sentMovements.ToImmutableList();

	public Task SendMovementAsync(Direction direction, int playerId)
	{
		_sentMovements.Add(direction);

		return Task.CompletedTask;
	}
}
