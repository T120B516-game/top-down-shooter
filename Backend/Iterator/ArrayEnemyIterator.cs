using Shared;

namespace Backend.Iterator
{
    public class ArrayIterator : IIterator<Enemy>
    {
        private readonly Enemy[] _array;
        private int _position;

        public ArrayIterator(Enemy[] array)
        {
            _array = array;
            _position = 0;
        }

        public bool HasNext()
        {
            return _position < _array.Length && _array[_position] != null;
        }

        public Enemy Next()
        {
            if (!HasNext())
                throw new InvalidOperationException("No more elements.");

            return _array[_position++];
        }
    }
}
