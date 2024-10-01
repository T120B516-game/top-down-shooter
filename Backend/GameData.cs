using Backend.Entities;

namespace Backend
{
	/// <summary>
	/// Class that contains all game data
	/// </summary>
	static public class GameData
	{
		static public int Speed = 10;
		static private int nextID = 0;
		static public int NextID
		{
			get => nextID++;
		}
		static public List<Player> Players { get; set; } = new List<Player>();
	}
}
