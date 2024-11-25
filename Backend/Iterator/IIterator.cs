namespace Backend.Iterator
{
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }
}
