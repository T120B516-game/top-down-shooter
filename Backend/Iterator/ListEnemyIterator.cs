using Shared;

namespace Backend.Iterator
{
    public class ListIterator : IIterator<Enemy>
    {
        private readonly List<Enemy> _list;
        private int _position;

        public ListIterator(List<Enemy> list)
        {
            _list = list;
            _position = 0;
        }

        public bool HasNext()
        {
            return _position < _list.Count;
        }

        public Enemy Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements.");

            return _list[_position++];
        }
    }
}
