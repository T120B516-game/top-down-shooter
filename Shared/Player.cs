namespace Shared;

public class Player : IRenderable
{
	public required int Id { get; set; }
	public required int X { get; set; }
	public required int Y { get; set; }
	public required string Image { get; set; }
	public required int Health { get; set; }
	public required int Speed { get; set; }
	public required string Direction { get; set; }
}
