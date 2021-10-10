using System;

namespace Stash.Serialization.Converters
{
    public class BooleanConverter : BsonConverter<bool>
    {
        public override bool Read(ref BsonReader reader, Type typeToConvert, BsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(BsonWriter writer, bool value, BsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
