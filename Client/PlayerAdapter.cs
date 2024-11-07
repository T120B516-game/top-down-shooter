using Shared;
using System.Drawing;

namespace Client
{
    public class PlayerAdapter : PlayerComponent
    {
        private readonly Player _player;
        public int Id => _player.Id;
        public int X => _player.X;
        public int Y => _player.Y;

        public PlayerAdapter(Player player)
        {
            _player = player;
        }

        public void Draw(Graphics g)
        {
            var image = (Bitmap)Sprites.ResourceManager.GetObject(_player.Image);
            g.DrawImage(image, X, Y);
        }


    }
}
