using Shared;

namespace Client
{
    public class ObstacleRepository
    {
        private readonly ObstacleFactory _factory;

        public ObstacleRepository()
        {
            _factory = new ObstacleFactory();
        }

        /// <summary>
        /// Creates a set of default obstacles and updates the shared repository.
        /// </summary>
        public IEnumerable<Obstacle> CreateDefaultObstacles()
        {
            var obstacles = new List<Obstacle>
            {
                _factory.CreatePenetratable(300, 500, 100, 100),
                _factory.CreateUnpenetratable(400, 500, 100, 100)
            };

            Shared.ObstacleRepository.AddObstacle(obstacles); // Updates shared repository
            return obstacles;
        }
    }
}
