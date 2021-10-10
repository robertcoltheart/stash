using System;

namespace Stash.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BsonIgnoreAttribute : Attribute
    {
    }
}
