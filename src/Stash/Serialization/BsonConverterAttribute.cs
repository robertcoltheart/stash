using System;

namespace Stash.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BsonConverterAttribute : Attribute
    {
        public BsonConverterAttribute(Type converterType)
        {
            ConverterType = converterType;
        }

        public Type? ConverterType { get; }

        public virtual BsonConverter? CreateConverter(Type typeToConvert)
        {
            return null;
        }
    }
}
