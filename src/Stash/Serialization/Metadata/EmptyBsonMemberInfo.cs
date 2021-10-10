using System;

namespace Stash.Serialization.Metadata
{
    public class EmptyBsonMemberInfo : BsonMemberInfo
    {
        public override string Name { get; } = string.Empty;

        public override Type MemberType { get; } = typeof(string);

        public override object? GetValue(object resource)
        {
            return null;
        }

        public override void SetValue(object resource, object? value)
        {
        }

        public override void Read(ref BsonReader reader, object resource)
        {
        }

        public override bool Write(BsonWriter writer, object resource)
        {
            return false;
        }
    }
}
