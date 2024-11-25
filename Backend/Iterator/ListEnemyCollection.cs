using Shared;

namespace Backend.Iterator
{
    public class ListEnemyCollection : IEnemyCollection
    {
        private readonly List<Enemy> _enemies = new();

        public void Add(Enemy enemy)
        {
            _enemies.Add(enemy);
        }

        public void Remove(Enemy enemy)
        {
            _enemies.Remove(enemy);
        }

        public IIterator<Enemy> GetIterator()
        {
            return new ListIterator(_enemies);
        }
    }

}
