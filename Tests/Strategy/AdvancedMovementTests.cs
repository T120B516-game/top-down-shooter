using Backend.CreationalPatterns;
using Shared;

namespace Tests.Strategy;

public class AdvancedMovementTests
{
    [Fact]
    public void AdvancedMovement_ShouldMoveEnemy_TowardsPlayer()
    {
        var enemy = new MobileExplosiveEnemy(5, 5, 100, 1);
        enemy.SetMovementBehaviour(new AdvancedMovement());
        var player = new Player()
        {
            Id = 1,
            X = 7,
            Y = 5,
            Image = "PlayerUp",
            Health = 40,
            Speed = 10,
        };
        var players = new List<Player> { player };

        enemy.PerformMovement(players);

        Assert.Equal(6, enemy.X);
        Assert.Equal(5, enemy.Y);
    }

    [Fact]
    public void AdvancedMovement_ShouldMoveEnemy_TowardsClosestPlayer()
    {
        var enemy = new MobileExplosiveEnemy(5, 5, 100, 1);
        enemy.SetMovementBehaviour(new AdvancedMovement());

        var player1 = new Player()
        {
            Id = 10,
            X = 10,
            Y = 10,
            Image = "PlayerUp",
            Health = 40,
            Speed = 10,
        };
        var player2 = new Player()
        {
            Id = 3,
            X = 3,
            Y = 3,
            Image = "PlayerUp",
            Health = 40,
            Speed = 10,
        };

        var players = new List<Player>
    {
        player1,
        player2
    };

        enemy.PerformMovement(players);

        Assert.InRange(enemy.X, 4, 5);
        Assert.InRange(enemy.Y, 4, 5);
    }

    [Fact]
    public void AdvancedMovement_ShouldNotMoveEnemy_WhenNoPlayersArePresent()
    {
        var enemy = new MobileExplosiveEnemy(5, 5, 100, 1);
        enemy.SetMovementBehaviour(new AdvancedMovement());
        var initialX = enemy.X;
        var initialY = enemy.Y;
        
        enemy.PerformMovement(new List<Player>());
        
        Assert.Equal(initialX, enemy.X);
        Assert.Equal(initialY, enemy.Y);
    }

    [Fact]
    public void AdvancedMovement_ShouldNotMove_WhenNoPlayers()
    {
        var enemy = new MobileExplosiveEnemy(5, 5, 100, 1);
        enemy.SetMovementBehaviour(new AdvancedMovement());
        var initialX = enemy.X;
        var initialY = enemy.Y;

        enemy.PerformMovement(new List<Player>());

        Assert.Equal(initialX, enemy.X);
        Assert.Equal(initialY, enemy.Y);
    }
}
