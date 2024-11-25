using Shared;

namespace Backend.Iterator
{
    public class HashMapIterator : IIterator<Enemy>
    {
        private readonly Dictionary<int, Enemy> _map;
        private readonly IEnumerator<KeyValuePair<int, Enemy>> _enumerator;

        public HashMapIterator(Dictionary<int, Enemy> map)
        {
            _map = map;
            _enumerator = _map.GetEnumerator();
        }

        public bool HasNext()
        {
            return _enumerator.MoveNext();
        }

        public Enemy Next()
        {
            return _enumerator.Current.Value;
        }
    }

}
