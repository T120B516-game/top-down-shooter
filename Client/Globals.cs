using Shared;

namespace Client;

public static class Globals
{
	public const string BACKEND_BASE_URL = "http://localhost:5270";
	static public int PersonalID = -1;

	public static int[,] ColliderMap = new int[1200, 800];
	public static PlayerAdapter ThisPlayer = new PlayerAdapter();
	public static Form1 Form { get; set; }
}
