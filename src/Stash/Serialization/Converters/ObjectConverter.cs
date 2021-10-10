using System;

namespace Stash.Serialization.Converters
{
    public class ObjectConverter<T> : BsonConverter<T>
        where T : notnull
    {
        public override T? Read(ref BsonReader reader, Type typeToConvert, BsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(BsonWriter writer, T value, BsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
