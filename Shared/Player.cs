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
}
