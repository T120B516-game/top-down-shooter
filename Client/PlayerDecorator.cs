using System.Drawing;

namespace Client
{
    public abstract class PlayerDecorator : PlayerComponent
    {
        public PlayerComponent _component;



        public PlayerDecorator(PlayerComponent component)
        {
            _component = component;
        }

        public int Id => _component.Id;

        public int X => _component.X;

        public int Y => _component.Y;

        public virtual void Draw(Graphics g)
        {
            _component.Draw(g);
        }
    }
}
