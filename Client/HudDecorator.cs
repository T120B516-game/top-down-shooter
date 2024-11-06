using System.Drawing;

namespace Client
{
    public class HudDecorator : PlayerDecorator
    {
        private readonly int _health;

        public HudDecorator(PlayerComponent component, int health) : base(component)
        {
            _health = health;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            using (Font font = new Font("Arial", 10))
            using (Brush brush = new SolidBrush(Color.Red))
            {
                g.DrawString($"Health: {_health}", font, brush, new PointF(_component.X + 50, _component.Y + 50));
            }
        }
    }
}
