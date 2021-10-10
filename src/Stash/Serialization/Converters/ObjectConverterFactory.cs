using System;

namespace Stash.Serialization.Converters
{
    public class ObjectConverterFactory : BsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override BsonConverter? CreateConverter(Type typeToConvert, BsonSerializerOptions options)
        {
            var converterType = typeof(ObjectConverter<>).MakeGenericType(typeToConvert);

            return (BsonConverter)Activator.CreateInstance(converterType);
        }
    }
}
