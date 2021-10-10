using System;
using System.Reflection;

namespace Stash.Serialization.Metadata
{
    public sealed class BsonPropertyInfo<T> : BsonMemberInfo<T>
    {
        public BsonPropertyInfo(PropertyInfo property, BsonConverter converter, BsonSerializerOptions options)
            : base(property, property.PropertyType, converter, options)
        {
            Get = CreateGetter(property);
            Set = CreateSetter(property);
        }

        public override Func<object, T>? Get { get; }

        public override Action<object, T>? Set { get; }

        private Func<object, T>? CreateGetter(PropertyInfo property)
        {
            if (!IsPublic(property.GetMethod))
            {
                return null;
            }

            return Options.MemberAccessor.CreatePropertyGetter<T>(property);
        }

        private Action<object, T>? CreateSetter(PropertyInfo property)
        {
            if (!IsPublic(property.SetMethod))
            {
                return null;
            }

            return Options.MemberAccessor.CreatePropertySetter<T>(property);
        }

        private bool IsIgnored(PropertyInfo property)
        {
            return IsReadOnly(property);
        }

        private bool IsReadOnly(PropertyInfo property)
        {
            return IsPublic(property.GetMethod) && !IsPublic(property.SetMethod);
        }
    }
}
