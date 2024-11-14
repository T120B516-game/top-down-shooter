using Shared;

namespace Tests.Strategy;

public class AdvancedMovementTests
{
    [Fact]
    public void AdvancedMovement_ShouldMoveEnemy_TowardsClosestPlayer()
    {
        var enemy = new Enemy(5, 5, 100, 1) { MovementBehaviour = new AdvancedMovement() };
        var player = new Player { X = 7, Y = 5 };
        var players = new List<Player> { player };
        
        enemy.PerformMovement(players);
        
        Assert.Equal(6, enemy.X);
        Assert.Equal(5, enemy.Y);
    }

    [Fact]
    public void AdvancedMovement_ShouldNotMoveEnemy_WhenNoPlayersArePresent()
    {
        var enemy = new Enemy(5, 5, 100, 1) { MovementBehaviour = new AdvancedMovement() };
        var initialX = enemy.X;
        var initialY = enemy.Y;
        
        enemy.PerformMovement(new List<Player>());
        
        Assert.Equal(initialX, enemy.X);
        Assert.Equal(initialY, enemy.Y);
    }
}
