using System;
using System.Reflection;

namespace Stash.Serialization.Metadata
{
    internal abstract class MemberAccessor
    {
        public abstract Func<object?> CreateCreator(Type type);

        public abstract Func<object, T> CreatePropertyGetter<T>(PropertyInfo property);

        public abstract Action<object, T> CreatePropertySetter<T>(PropertyInfo property);
    }
}
