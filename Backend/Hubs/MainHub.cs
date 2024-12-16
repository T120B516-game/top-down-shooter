using Backend.BehavioralPatterns;
using Microsoft.AspNetCore.SignalR;
using Shared;
using System.Collections.Concurrent;

namespace Backend.Hubs;

public class MainHub : Hub
{
    private readonly PlayerRepository _playerRepository;
    private readonly EnemyRepository _enemyRepository;
    private readonly GameUpdater _gameUpdater;
    private readonly PlayerController _playerController;

    public MainHub(
        PlayerRepository playerRepository,
        EnemyRepository enemyRepository,
        GameUpdater gameUpdater,
        PlayerController playerController)
    {
        _playerRepository = playerRepository;
        _enemyRepository = enemyRepository;
        _gameUpdater = gameUpdater;
        _playerController = playerController;
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
    /// Undo (5) direction is added for testing
    /// </summary>
    /// <param name="direction">Integer which references direction (1 - up, 2 - right, 3 - down, 4 - left)</param>
    /// <param name="playerId">The id of the player, whose coordinates need to be changed</param>
    [HubMethodName("MovePlayer")]
    public async Task MovePlayerAsync(int direction, int playerId)
    {
        var player = await _playerRepository.GetAsync(playerId);
        if (player == null)
        {
            return;
        }

        switch (direction)
        {
            case 1:
                _playerController.MoveUp(player);
                break;
            case 2:
                _playerController.MoveRight(player);
                break;
            case 3:
                _playerController.MoveDown(player);
                break;
            case 4:
                _playerController.MoveLeft(player);
                break;
            case 5:
                _playerController.Undo();
                break;
        }
    }

    /// <summary>
    /// Teleports the player to specific coordinates
    /// </summary>
    /// <param name="x">The X coordinate</param>
    /// <param name="y">The Y coordinate</param>
    /// <param name="playerId">The ID of the player to teleport</param>
    [HubMethodName("TeleportPlayer")]
    public async Task TeleportPlayerAsync(int x, int y, int playerId)
    {
        var player = await _playerRepository.GetAsync(playerId);
        if (player == null)
        {
            return;
        }

        // Update the player's position with the new coordinates (teleportation)
        player.X = x;
        player.Y = y;

        // Broadcast the teleportation to all clients
        //await Clients.All.SendAsync("PlayerTeleported", playerId, x, y);
    }

    [HubMethodName("DamageEnemy")]
    public async Task DamageEnemy(int enemyId)
    {
        var enemies = await _enemyRepository.ListAsync();
        var enemy = enemies.Where(e => e.Id == enemyId).FirstOrDefault();
        if (enemy is not null)
        {
            if (enemy.Health > 20)
            {
                enemy.Health -= 20;
            }
            else
            {
                _enemyRepository.Remove(enemy);
                await Clients.All.SendAsync("RemoveEnemy", enemyId);
            }
        }
    }

    [HubMethodName("BulletFired")]
    public async Task BulletFired(int playerId)
    {
		var player = await _playerRepository.GetAsync(playerId);
        if(player is not null)
            await Clients.All.SendAsync("SpawnBullet", player.Id, player.X, player.Y, player.Direction);
	}
}
