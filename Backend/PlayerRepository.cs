﻿using Shared;
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
		_semaphorSlim.Release();

		var player = new Player()
		{
			Id = _lastId,
			X = 100,
			Y = 100,
			Image = "PlayerUp",
			Health = 100,
			Speed = 10,
		};
		_players[_lastId] = player;

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