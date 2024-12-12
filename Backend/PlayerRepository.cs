using Shared;
using System.Collections.Concurrent;

namespace Backend;

public class PlayerRepository
{
	private int _lastId = -1;
	private readonly ConcurrentDictionary<int, Player> _players = [];
	private readonly SemaphoreSlim _semaphorSlim = new(1, 1);

	public async Task<Player> CreateAsync()
	{
		await _semaphorSlim.WaitAsync();
		
		_lastId++;
		var player = new Player()
		{
			Id = _lastId,
			X = 100,
			Y = 100,
			Image = "PlayerUp",
			Health = 40,
			Speed = 10,
		};
		_players[_lastId] = player;

		_semaphorSlim.Release();

        var loggingVisitor = new LoggingVisitor();
        var buffVisitor = new BuffVisitor(10, 5);
        player.Accept(buffVisitor);
        player.Accept(loggingVisitor);

        return player;
	}

	public async Task<Player?> GetAsync(int id)
	{
		return _players[id];
	}

	public async Task<List<Player>> ListAsync()
	{
		return _players.Values.ToList();
	}
}
