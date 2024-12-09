using Shared;
using System.Drawing;
using Client.Observer;

namespace Client
{
    public class PlayerAdapter : CollisionTemplate
    {
        private Player _player;
		public PlayerAdapter(Player player) => _player = player;

		public PlayerAdapter() => _player = null;

		protected override bool DetectCollision(Form form, out Control? collider)
		{
			collider = null;
			int playerX = _player.X;
			int size = 100;
			int playerY = _player.Y;
			for (int x = playerX; x < playerX + size; x++)
			{
				for (int y = playerY; y < playerY + size; y++)
				{
					if (Globals.ColliderMap[x, y] != 0)
						return true;
				}
			}
			return false;
		}

		public void UpdatePlayer(Player player)
		{
			_player = player;
		}

		protected override void InteractWithCollider(Control collider)
		{
			return;
		}

		protected override void ResponseToCollision(object param)
		{
			return;
		}

		public void UpdatePosition(Direction direction)
		{
			if (direction == Direction.Up)
				_player.Y -= _player.Speed;
			if (direction == Direction.Down)
				_player.Y += _player.Speed;
			if (direction == Direction.Left)
				_player.X -= _player.Speed;
			if (direction == Direction.Right)
				_player.X += _player.Speed;
		}
	}
}
