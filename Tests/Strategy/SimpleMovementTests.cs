using Shared;

namespace Tests.Strategy;

public class SimpleMovementTests
{
    [Fact]
    public void SimpleMovement_ShouldMoveEnemy_RandomlyWithinBounds()
    {
        var enemy = new MobileExplosiveEnemy(5, 5, 100, 1);
        enemy.SetMovementBehaviour(new SimpleMovement());
        var initialX = enemy.X;
        var initialY = enemy.Y;
        
        enemy.PerformMovement(new List<Player>());
        
        Assert.InRange(enemy.X, initialX - 1, initialX + 1);
        Assert.InRange(enemy.Y, initialY - 1, initialY + 1);
    }

    [Fact]
    public void MobileShootingEnemy_ShouldMoveRandomlyWithinBounds_WithSimpleMovement()
    {
        var enemy = new MobileShootingEnemy(5, 5, 100, 2);
        enemy.SetMovementBehaviour(new SimpleMovement());
        var initialX = enemy.X;
        var initialY = enemy.Y;

        enemy.PerformMovement(new List<Player>());

        Assert.InRange(enemy.X, initialX - 1, initialX + 1);
        Assert.InRange(enemy.Y, initialY - 1, initialY + 1);
    }

    [Fact]
    public void Enemy_ShouldSetMovementBehaviour_Correctly()
    {
        var enemy = new MobileExplosiveEnemy(5, 5, 100, 1);
        var simpleMovement = new SimpleMovement();
        var advancedMovement = new AdvancedMovement();

        enemy.SetMovementBehaviour(simpleMovement);
        Assert.IsType<SimpleMovement>(enemy.MovementBehaviour);

        enemy.SetMovementBehaviour(advancedMovement);
        Assert.IsType<AdvancedMovement>(enemy.MovementBehaviour);
    }
}