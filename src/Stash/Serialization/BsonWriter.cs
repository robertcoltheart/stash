using System;
using System.Buffers;

namespace Stash.Serialization
{
    public class BsonWriter : IDisposable
    {
        private readonly IBufferWriter<byte> bufferWriter;

        private Memory<byte> memory;

        private int currentDepth;

        public BsonWriter(IBufferWriter<byte> bufferWriter)
        {
            this.bufferWriter = bufferWriter;
        }

        public int BytesPending { get; private set; }

        public long BytesCommitted { get; private set; }

        public void Dispose()
        {
        }

        public void WriteStartObject()
        {
            //TODO Write doc length

            var span = memory.Span;
            span[BytesPending++] = 0x03;

            currentDepth++;
        }

        public void WriteEndObject()
        {
            // TODO Write length stored in stack
            if (currentDepth != 0)
            {
                currentDepth--;
            }
        }

        public void WriteNumber(string propertyName, int value)
        {
        }

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value)
        {
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, int value)
        {
        }

        private void Grow(int requiredSize)
        {
            memory = bufferWriter.GetMemory(requiredSize);
        }
    }
}
