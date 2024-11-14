using Shared;

namespace Tests.Strategy;

public class SimpleMovementTests
{
    [Fact]
    public void SimpleMovement_ShouldMoveEnemy_RandomlyWithinBounds()
    {
        var enemy = new Enemy(5, 5, 100, 1) { MovementBehaviour = new SimpleMovement() };
        var initialX = enemy.X;
        var initialY = enemy.Y;
        
        enemy.PerformMovement(new List<Player>());
        
        Assert.InRange(enemy.X, initialX - 1, initialX + 1);
        Assert.InRange(enemy.Y, initialY - 1, initialY + 1);
    }
}