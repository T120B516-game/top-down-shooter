using Shared;
using System.Collections.Generic;

namespace Backend.Iterator
{
    public class ListEnemyCollection : IEnemyCollection
    {
        private readonly List<Enemy> _enemies = new();
        private int _position;

        public ListEnemyCollection()
        {
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

        public void Start()
        {
            _position = 0;
        }

        public Enemy Next()
        {
            if (!HasNext())
                return null;
            return _enemies[_position++];
        }
    }

}
