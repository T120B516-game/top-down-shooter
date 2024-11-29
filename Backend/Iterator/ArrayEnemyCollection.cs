using Shared;

namespace Backend.Iterator
{
    public class ArrayEnemyCollection : IEnemyCollection
    {
        private readonly Enemy[] _enemies;
        private int _currentIndex;

        public ArrayEnemyCollection()
        {
            _enemies = new Enemy[10];
            _currentIndex = 0;
        }

        public void Add(Enemy enemy)
        {
            if (_currentIndex >= _enemies.Length)
                throw new InvalidOperationException("Array is full. Cannot add more enemies.");

            _enemies[_currentIndex++] = enemy;
        }

        public void Remove(Enemy enemy)
        {
            for (int i = 0; i < _enemies.Length; i++)
            {
                if (_enemies[i] == enemy)
                {
                    _enemies[i] = null;

                    // Shift elements left
                    for (int j = i; j < _enemies.Length - 1; j++)
                    {
                        _enemies[j] = _enemies[j + 1];
                    }

                    _enemies[^1] = null;
                    _currentIndex--;
                    return;
                }
            }
        }

        public bool HasNext()
        {
            return _currentIndex < _enemies.Length && _enemies[_currentIndex] != null;
        }

        public Enemy Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements.");

            return _enemies[_currentIndex++];
        }
    }

}
