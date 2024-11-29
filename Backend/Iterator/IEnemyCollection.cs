using Shared;

namespace Backend.Iterator
{
    public interface IEnemyCollection
    {
        void Add(Enemy enemy);
        void Remove(Enemy enemy);
        bool HasNext();
        Enemy Next();
    }

}
