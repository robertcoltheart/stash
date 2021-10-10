using System;

namespace Stash.Serialization
{
    public abstract class BsonConverter<T> : BsonConverter
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(T);
        }

        public abstract T? Read(ref BsonReader reader, Type typeToConvert, BsonSerializerOptions options);

        public abstract void Write(BsonWriter writer, T value, BsonSerializerOptions options);
    }

    public abstract class BsonConverter
    {
        internal BsonConverter()
        {
        }

        public abstract bool CanConvert(Type typeToConvert);
    }
}
