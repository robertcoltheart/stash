using System;
using System.Linq;

namespace Stash.Collections
{
    public static class StashCollectionExtensions
    {
        public static IQueryable<T> AsQueryable<T>(this IStashCollection<T> collection)
        {
            throw new NotImplementedException();
        }
    }
}
