using Shared;
using System.Drawing;

namespace Backend
{
    public class ObstacleRepository
    {
        static public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();
        static public ObstacleFactory ObstacleFactory { get; set; } = new ObstacleFactory();

        //temporary
        static ObstacleRepository()
        {
            var factory = new ObstacleFactory();

            var penetratable = factory.CreatePenetratableBuilder()
                .SetPosition(500, 100)
                .SetSize(50, 50)
                .Build();
            Obstacles.Add(penetratable);

            var unpenetratable = factory.CreateUnpenetratableBuilder()
                .SetPosition(300, 300)
                .SetSize(50, 50)
                .Build();
            Obstacles.Add(unpenetratable);
        }
    }
}
