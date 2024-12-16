using Shared;

namespace Backend.Iterator
{
    public class EnemyIteratorRepository
    {
        private readonly IEnemyCollection _enemyCollection;

        public EnemyIteratorRepository(string dataStructureType)
        {
            _enemyCollection = dataStructureType.ToLower() switch
            {
                "list" => new ListEnemyCollection(),
                "array" => new ArrayEnemyCollection(),
                "hashmap" => new HashMapEnemyCollection(),
                _ => throw new ArgumentException("Unsupported data structure type. Choose 'list', 'array', or 'hashmap'.")
            };
        }

        public IEnemyCollection GetIterator()
        {
            return _enemyCollection;
        }

        public void Remove(Enemy enemy)
        {
            _enemyCollection.Remove(enemy);
        }
    }
}
