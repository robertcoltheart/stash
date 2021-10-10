using System;

namespace Stash.Serialization
{
    public abstract class BsonConverterFactory : BsonConverter
    {
        public abstract BsonConverter? CreateConverter(Type typeToConvert, BsonSerializerOptions options);
    }
}
