using Shared;

namespace Backend
{
    public class ObstacleRepository
    {
        static public List<Obstacle> Obstacles { get; set; } = new List<Obstacle>();
        static public ObstacleFactory ObstacleFactory { get; set; } = new ObstacleFactory();

        //temporary
        static ObstacleRepository()
        {
            var penetratable = ObstacleFactory.CreateObstacle("Penetratable");
            penetratable.X = 500;
            penetratable.Y = 100;
            penetratable.Width = 50;
            penetratable.Height = 50;
            penetratable.Type = "Penetratable";
            Obstacles.Add(penetratable);

            var unpenetratable = ObstacleFactory.CreateObstacle("Unpenetratable");
            unpenetratable.X = 300;
            unpenetratable.Y = 300;
            unpenetratable.Width = 50;
            unpenetratable.Height = 50;
            unpenetratable.Type = "Unpenetratable";
            Obstacles.Add(unpenetratable);
        }
    }
}
