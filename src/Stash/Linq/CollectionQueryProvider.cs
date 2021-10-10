using System.Linq;
using System.Linq.Expressions;

namespace Stash.Linq
{
    public class CollectionQueryProvider : IQueryProvider
    {
        public IQueryable CreateQuery(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public object Execute(Expression expression)
        {
            throw new System.NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            throw new System.NotImplementedException();
        }
    }
}
