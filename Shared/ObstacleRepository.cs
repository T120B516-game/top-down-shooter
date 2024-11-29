namespace Shared
{
    public class ObstacleRepository
    {
        private static readonly List<Obstacle> _obstacles = new();

        public static List<Obstacle> Obstacles => _obstacles;

        public static void AddObstacle(IEnumerable<Obstacle> obstacles)
        {
            Obstacles.AddRange(obstacles);
        }
    }
}
