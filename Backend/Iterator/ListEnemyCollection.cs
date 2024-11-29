using Shared;

namespace Backend.Iterator
{
    public class ListEnemyCollection : IEnemyCollection
    {
        private readonly List<Enemy> _enemies;
        private int _position;

        public ListEnemyCollection()
        {
            _enemies = new List<Enemy>();
            _position = 0;
        }

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void Remove(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        public bool HasNext()
        {
            return _position < _enemies.Count;
        }

        public Enemy Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements.");

            return _enemies[_position++];
        }
    }

}
