using System.Drawing;

namespace Client
{
    public class AppearanceDecorator : PlayerDecorator
    {
        private readonly Color _colorOverlay;

        public AppearanceDecorator(PlayerComponent component, Color colorOverlay) : base(component)
        {
            _colorOverlay = colorOverlay;
        }

        public override void Draw(Graphics g)
        {
            base.Draw(g);
            using (Brush overlayBrush = new SolidBrush(Color.FromArgb(50, _colorOverlay)))
            {
                g.FillRectangle(overlayBrush, new Rectangle(_component.X, _component.Y, 100, 100));
            }
        }
    }
}
