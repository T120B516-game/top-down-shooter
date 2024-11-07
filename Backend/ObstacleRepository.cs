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

            var penetratable = factory.CreatePenetratable(500, 500, 50, 50);
           Obstacles.Add(penetratable);

            var unpenetratable = factory.CreateUnpenetratable(300, 300, 50, 50);
            Obstacles.Add(unpenetratable);
        }
    }
}
