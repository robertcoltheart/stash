namespace Stash.Serialization
{
    public static class BsonSerializer
    {
        public static void Serialize<T>(T value, BsonSerializerOptions? options = null)
        {
            options ??= BsonSerializerOptions.Default;

            using var buffer = new PooledByteWriterBuffer();
            using var writer = new BsonWriter(buffer);

            var converter = options.GetConverter(typeof(T));
        }
    }
}
