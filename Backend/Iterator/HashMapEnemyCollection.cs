using Shared;

namespace Backend.Iterator
{
    public class HashMapEnemyCollection : IEnemyCollection
    {
        private readonly Dictionary<int, Enemy> _enemies = new();
        private readonly IEnumerator<KeyValuePair<int, Enemy>> _enumerator;
        private int _keyCounter;

        public HashMapEnemyCollection()
        {
            _enumerator = _enemies.GetEnumerator();
        }

        public void Add(Enemy enemy)
        {
            _enemies[++_keyCounter] = enemy;
        }

        public void Remove(Enemy enemy)
        {
            var keyToRemove = -1;
            foreach (var kvp in _enemies)
            {
                if (kvp.Value == enemy)
                {
                    keyToRemove = kvp.Key;
                    break;
                }
            }

            if (keyToRemove != -1)
                _enemies.Remove(keyToRemove);
        }

        public bool HasNext()
        {
            return _enumerator.MoveNext();
        }

        public void Start()
        {
            _enumerator.Reset();
        }

        public Enemy Next()
        {
            return _enumerator.Current.Value;
        }
    }

}
