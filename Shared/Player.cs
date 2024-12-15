namespace Shared;

public class Player : IRenderable, IVisitable
{
	public required int Id { get; set; }
	public required int X { get; set; }
	public required int Y { get; set; }
	public required string Image { get; set; }
	public required int Health { get; set; }
	public required int Speed { get; set; }

    public void Accept(IPlayerVisitor visitor)
    {
        visitor.Visit(this);
    }

    public void Accept(IInteractionVisitor visitor)
    {
        visitor.Visit(this);
    }

    public bool IsCollidingWith(Obstacle obstacle)
    {
        return X < obstacle.X + obstacle.Width &&
               X - 20 > obstacle.X &&
               Y < obstacle.Y + obstacle.Height &&
               Y - 20 > obstacle.Y;
    }

    public bool IsCollidingWith(Player player)
    {
        return X < player.X + 10 && // Right edge of this player is beyond the left edge of the other player
               X + 10 > player.X && // Left edge of this player is before the right edge of the other player
               Y < player.Y + 10 && // Bottom edge of this player is above the top edge of the other player
               Y + 10 > player.Y;   // Top edge of this player is below the bottom edge of the other player
    }

}
