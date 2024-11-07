namespace Shared;

public interface IMovementBehaviour
{
    void Move(Enemy enemy, List<Player> players);
}

public class SimpleMovement : IMovementBehaviour
{
	private readonly Random _random = new();

    public void Move(Enemy enemy, List<Player> players)
    {
        enemy.X += _random.Next(-1, 2);
        enemy.Y += _random.Next(-1, 2);
    }
}

public class AdvancedMovement : IMovementBehaviour
{
    public void Move(Enemy enemy, List<Player> players)
    {
        var closestPlayer = players
            .OrderBy(p => CalculateDistance(p, enemy))
            .FirstOrDefault();
        
        if (closestPlayer != null)
        {
            if (enemy.X < closestPlayer.X) enemy.X++;
            if (enemy.X > closestPlayer.X) enemy.X--;
            if (enemy.Y < closestPlayer.Y) enemy.Y++;
            if (enemy.Y > closestPlayer.Y) enemy.Y--;
        }
    }

    private static double CalculateDistance(Player player, Enemy enemy)
    {
        var xDistance = Math.Pow(player.X - enemy.X, 2);
        var yDistance = Math.Pow(player.Y - enemy.Y, 2);

		return Math.Sqrt(xDistance + yDistance);
	}
}
