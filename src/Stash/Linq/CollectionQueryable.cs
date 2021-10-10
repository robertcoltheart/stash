using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Stash.Linq
{
    public class CollectionQueryable<T> : IQueryable<T>
    {
        public CollectionQueryable()
        {
            Expression = Expression.Constant(this, typeof(IQueryable<T>));
        }

        public Type ElementType { get; } = typeof(T);

        public Expression Expression { get; }

        public IQueryProvider Provider { get; } = new CollectionQueryProvider();

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)Provider.Execute(Expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
