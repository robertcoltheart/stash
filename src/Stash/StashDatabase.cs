using System;
using Stash.Collections;

namespace Stash
{
    public class StashDatabase : IStashDatabase
    {
        public IStashCollection<T> GetCollection<T>(string name)
        {
            throw new NotImplementedException();
        }
    }
}
