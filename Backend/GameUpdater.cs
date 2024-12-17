using Backend.ChainOfResponsibility;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Shared;
using System.Numerics;


namespace Backend;

/// <summary>
/// Main update cycle of the game
/// </summary>
public class GameUpdater
{
    private readonly PlayerRepository _playerRepository;
    private readonly IEnemyRepository _enemyRepository;
    private readonly ObstacleRepository _obstacleRepository;
    private readonly ILoggerHandler _loggerChain;

    private IHubCallerClients? _clients = null;
    private Task _broadcastingTask;
    private bool _shouldUpdateObstacles = true;

    public GameUpdater(PlayerRepository playerRepository, IEnemyRepository enemyRepository, ObstacleRepository obstacleRepository)
    {
        _playerRepository = playerRepository;
        _enemyRepository = enemyRepository;
        _obstacleRepository = obstacleRepository;

        if (!Directory.Exists("logs"))
        {
            Directory.CreateDirectory("logs");
        }

        // Set up the logger chain
        var consoleLogger = new ConsoleLogger();
        var xmlLogger = new XmlLogger("logs/xmlLog.txt", consoleLogger);
        var jsonLogger = new JsonLogger("logs/jsonLog.txt", xmlLogger);
        var textLogger = new TextLogger("logs/textLog.txt", jsonLogger);

        _loggerChain = textLogger; // Starting point of the chain

    }

    /// <summary>
    /// Once this function is called, BroadcastGameUpdate starts getting invoked repeatedly (every _updateInterval), broadcasting the game data to all clients
    /// </summary>
    public void Start(IHubCallerClients hubContext)
    {
        if (_clients is not null) return;
        _clients = hubContext;

        AddEnemiesForTesting();
        _broadcastingTask = LoopBroadcastAsync();
    }

    private void AddEnemiesForTesting()
    {
        _enemyRepository.Add(5, 200, 500, "mobile", "meele");
        _enemyRepository.Add(6, 200, 700, "mobile", "shooting");
		_enemyRepository.Add(7, 300, 550, "stationary", "shooting");
	}

	private async Task LoopBroadcastAsync()
    {
        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(20));

        while (await timer.WaitForNextTickAsync())
        {
            var players = await _playerRepository.ListAsync();
            var enemies = await _enemyRepository.ListAsync();
            var obstacles = await _obstacleRepository.ListAsync();

            // Log data
            //var logMessage = $"Players: {players.Count}, Enemies: {enemies.Count}";
            //_loggerChain.Log(logMessage);

            var enemiesJson = JsonConvert.SerializeObject(enemies);
            var playersJson = JsonConvert.SerializeObject(players);

            foreach (var player in players)
            {
                foreach (var otherPlayer in players)
                {
                    if (player.Id != otherPlayer.Id && player.IsCollidingWith(otherPlayer))
                    {
                        var interactionVisitor = new InteractionVisitor(player);
                        otherPlayer.Accept(interactionVisitor);
                    }
                }
            }

            foreach (var enemy in enemies)
            {
                enemy.UpdateAI(players);
            }

            if (_clients != null)
            {
                if (_shouldUpdateObstacles) // Only update if there are changes
                {
                    var obstacleData = JsonConvert.SerializeObject(ObstacleRepository.Obstacles);

                    await _clients.All.SendAsync("ReceiveObstaclesUpdate", obstacleData);
                    _shouldUpdateObstacles = false; // Reset the flag
                }

                await _clients.All.SendAsync("ReceiveGameUpdate", playersJson, enemiesJson);
            }
        }
    }
    public void MarkObstaclesForUpdate() // Call this method when obstacles change
    {
        _shouldUpdateObstacles = true;
    }
}
