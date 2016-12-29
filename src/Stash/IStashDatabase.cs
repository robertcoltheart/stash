namespace Stash
{
    public interface IStashDatabase
    {
        byte[] Get(byte[] key);
        void Remove(byte[] key);
        void Set(byte[] key, byte[] value);
    }
}
