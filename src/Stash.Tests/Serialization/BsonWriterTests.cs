using System.Buffers;
using MongoDB.Bson;
using Stash.Serialization;
using Xunit;

namespace Stash.Tests.Serialization
{
    public class BsonWriterTests
    {
        [Fact]
        public void WriteDocument()
        {
            var model = new SimpleModel
            {
                Name = "BSON rocks"
            };

            var buffer = new ArrayBufferWriter<byte>();
            using var writer = new BsonWriter(buffer);

            writer.WriteStartObject();
            writer.WriteString(nameof(model.Name), model.Name);
            writer.WriteEndObject();
            writer.Flush();

            var expected = model.ToBson();
            var actual = buffer.WrittenSpan.ToArray();

            Assert.Equal(expected, actual);
        }

        private class SimpleModel
        {
            public string Name { get; set; }
        }
    }
}
