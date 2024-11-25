using Shared;

namespace Backend.Iterator
{
    public class HashMapEnemyCollection : IEnemyCollection
    {
        private readonly Dictionary<int, Enemy> _enemies = new();
        private int _keyCounter;

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

        public IIterator<Enemy> GetIterator()
        {
            return new HashMapIterator(_enemies);
        }
    }

}
