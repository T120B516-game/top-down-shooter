namespace Backend.Entities
{
	public class Player
	{
		public int Id;
		public int Horizontal;
		public int Vertical;
		public string Image;
		public int HP;

		public Player(int id, int horizontal = 100, int vertical = 100)
		{
			Id = id;
			Horizontal = horizontal;
			Vertical = vertical;
			Image = "PlayerUp";
			HP = 100;
		}
	}
}
