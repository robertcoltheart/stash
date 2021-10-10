using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Stash.Serialization.Converters;
using Stash.Serialization.Metadata;

namespace Stash.Serialization
{
    public class BsonSerializerOptions
    {
        internal static readonly BsonSerializerOptions Default = new();

        private static readonly BsonConverter[] DefaultConverters =
        {
            new ObjectConverterFactory()
        };

        private readonly ConcurrentDictionary<Type, BsonConverter?> converters = new();

        private readonly ConcurrentDictionary<Type, BsonTypeInfo> types = new();

        private MemberAccessor? memberAccessor;

        internal MemberAccessor MemberAccessor => memberAccessor ??= CreateMemberAccessor();

        public BsonConverter GetConverter(Type typeToConvert)
        {
            if (converters.TryGetValue(typeToConvert, out var converter))
            {
                return converter!;
            }

            foreach (var item in DefaultConverters)
            {
                if (item.CanConvert(typeToConvert))
                {
                    converter = item;
                    break;
                }
            }

            converters.TryAdd(typeToConvert, converter);

            return converter!;
        }

        public BsonTypeInfo GetTypeInfo(Type type)
        {
            if (!types.TryGetValue(type, out var value))
            {
                return types.GetOrAdd(type, x => new BsonTypeInfo(x, this));
            }

            return value;
        }

        private MemberAccessor CreateMemberAccessor()
        {
#if NETCOREAPP
            if (RuntimeFeature.IsDynamicCodeSupported)
            {
                return new EmitMemberAccessor();
            }

            return new ReflectionMemberAccessor();
#elif NETFRAMEWORK
            return new EmitMemberAccessor();
#else
            return new ReflectionMemberAccessor();
#endif
        }
    }
}
