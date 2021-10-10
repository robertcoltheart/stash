using System;

namespace Stash.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BsonPropertyNameAttribute : Attribute
    {
        public BsonPropertyNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
