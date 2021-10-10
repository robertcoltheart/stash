using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Stash.Serialization.Metadata
{
    public class BsonTypeInfo
    {
        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly EmptyBsonMemberInfo EmptyMember = new();

        private readonly Dictionary<string, BsonMemberInfo> nameCache;

        public BsonTypeInfo(Type type, BsonSerializerOptions options)
        {
            Creator = options.MemberAccessor.CreateCreator(type);

            var members = GetProperties(type, options)
                .ToArray();

            nameCache = members.ToDictionary(x => x.Name, StringComparer.Ordinal);
        }

        public Func<object?> Creator { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BsonMemberInfo GetMember(ReadOnlySpan<byte> name)
        {
            return nameCache.TryGetValue(name.GetString(), out var member)
                ? member
                : EmptyMember;
        }

        private IEnumerable<BsonMemberInfo> GetProperties(Type type, BsonSerializerOptions options)
        {
            var typeProperties = type
                .GetProperties(Flags)
                .Where(x => !x.GetIndexParameters().Any())
                .Where(x => x.GetMethod?.IsPublic == true || x.SetMethod?.IsPublic == true);

            foreach (var property in typeProperties)
            {
                yield return CreateMemberInfo(typeof(BsonPropertyInfo<>), property, property.PropertyType, options);
            }
        }

        private BsonMemberInfo CreateMemberInfo(Type memberInfoType, MemberInfo member, Type memberType, BsonSerializerOptions options)
        {
            var fieldType = memberInfoType.MakeGenericType(memberType);
            var converter = GetConverter(member, memberType, options);

            var fieldInfo = Activator.CreateInstance(fieldType, member, converter, options);

            if (fieldInfo is not BsonMemberInfo bsonMemberInfo)
            {
                throw new InvalidOperationException($"Cannot get type member info for '{member.Name}'");
            }

            return bsonMemberInfo;
        }

        private BsonConverter? GetConverter(MemberInfo member, Type memberType, BsonSerializerOptions options)
        {
            var converter = GetConverterAttribute(member);

            if (converter == null)
            {
                return options.GetConverter(memberType);
            }

            if (converter.ConverterType == null)
            {
                return converter.CreateConverter(memberType);
            }

            return Activator.CreateInstance(converter.ConverterType) as BsonConverter;
        }

        private BsonConverterAttribute? GetConverterAttribute(MemberInfo member)
        {
            var converters = member.GetCustomAttributes<BsonConverterAttribute>(false).ToArray();

            if (!converters.Any())
            {
                return null;
            }

            if (converters.Length > 1)
            {
                throw new InvalidOperationException($"The attribute 'JsonConverterAttribute' cannot exist more than once on '{member}'.");
            }

            return converters.First();
        }
    }
}
