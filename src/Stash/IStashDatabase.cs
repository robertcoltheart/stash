using Stash.Collections;

namespace Stash
{
    public interface IStashDatabase
    {
        IStashCollection<T> GetCollection<T>(string name);
    }
}
