using System;
using System.Reflection;

namespace Stash.Serialization.Metadata
{
    public abstract class BsonMemberInfo
    {
        public abstract string Name { get; }

        public abstract Type MemberType { get; }

        public abstract object? GetValue(object resource);

        public abstract void SetValue(object resource, object? value);

        public abstract void Read(ref BsonReader reader, object resource);

        public abstract bool Write(BsonWriter writer, object resource);
    }

    public abstract class BsonMemberInfo<T> : BsonMemberInfo
    {
        protected BsonMemberInfo(MemberInfo member, Type memberType, BsonConverter converter, BsonSerializerOptions options)
        {
            Name = GetName(member, options);
            MemberType = memberType;
            TypedConverter = (BsonConverter<T>) converter;
            Options = options;
        }

        public override string Name { get; }

        public override Type MemberType { get; }

        public BsonConverter<T> TypedConverter { get; }

        public BsonSerializerOptions Options { get; }

        public abstract Func<object, T>? Get { get; }

        public abstract Action<object, T>? Set { get; }

        public override object? GetValue(object resource)
        {
            if (Get == null)
            {
                return null;
            }

            return Get(resource);
        }

        public override void SetValue(object resource, object? value)
        {
            if (Set == null)
            {
                return;
            }

            if (value == null)
            {
                return;
            }

            Set(resource, (T) value!);
        }

        public override void Read(ref BsonReader reader, object resource)
        {
            if (Set == null)
            {
                return;
            }

            var value = TypedConverter.Read(ref reader, MemberType, Options);

            if (value == null)
            {
                return;
            }

            Set(resource, value!);
        }

        public override bool Write(BsonWriter writer, object resource)
        {
            if (Get == null)
            {
                return false;
            }

            var value = Get(resource);

            TypedConverter.Write(writer, value, Options);

            return true;
        }

        protected bool IsPublic(MethodInfo? method)
        {
            return method != null && method.IsPublic;
        }

        private string GetName(MemberInfo member, BsonSerializerOptions options)
        {
            var nameAttribute = member.GetCustomAttribute<BsonPropertyNameAttribute>(false);

            if (nameAttribute != null)
            {
                return nameAttribute.Name;
            }

            return member.Name;
        }
    }
}
