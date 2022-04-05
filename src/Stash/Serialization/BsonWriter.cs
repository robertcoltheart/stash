using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Text;

namespace Stash.Serialization
{
    public class BsonWriter : IDisposable
    {
        private IBufferWriter<byte>? bufferWriter;

        private Memory<byte> memory;

        private WriteStack stack;

        private int currentDepth;

        public BsonWriter(IBufferWriter<byte> bufferWriter)
        {
            this.bufferWriter = bufferWriter;
        }

        public int BytesPending { get; private set; }

        public int BytesCommitted { get; private set; }

        public void Dispose()
        {
            Flush();

            bufferWriter = null;
            memory = default;
            BytesCommitted = default;
            BytesPending = default;
            stack = default;
        }

        public void Flush()
        {
            memory = default;

            if (BytesPending > 0)
            {
                bufferWriter.Advance(BytesPending);
                BytesCommitted += BytesPending;
                BytesPending = 0;
            }
        }

        public void WriteStartObject()
        {
            Grow(4);

            var rootObject = currentDepth == 0;

            stack.Push();

            stack.Current.Depth = currentDepth + 1;
            stack.Current.Position = BytesCommitted + BytesPending;
            stack.Current.LengthMemory = memory.Slice(BytesPending, 4);

            BytesPending += 4;
            currentDepth++;

            if (!rootObject)
            {
                var span = memory.Span;
                span[BytesPending++] = 0x03;
            }
        }

        public void WriteStartArray()
        {
            currentDepth++;

            //TODO Write array length
            stack.Push();

            stack.Current.Depth = currentDepth;
            stack.Current.Position = BytesPending;
        }

        public void WriteEndObject()
        {
            Grow(1);

            memory.Span[BytesPending++] = 0x0;

            var output = stack.Current.LengthMemory;
            var objectLength = BytesCommitted + BytesPending - stack.Current.Position;

            BinaryPrimitives.WriteInt32LittleEndian(output.Span, objectLength);

            stack.Pop();

            if (currentDepth != 0)
            {
                currentDepth--;
            }
        }

        public void WriteNumber(string propertyName, int value)
        {
            var output = memory.Span;

            BinaryPrimitives.WriteInt32LittleEndian(output, value);

            BytesPending += 4;
        }

        public void WriteNumber(ReadOnlySpan<char> propertyName, int value)
        {
        }

        public void WriteNumber(ReadOnlySpan<byte> propertyName, int value)
        {
        }

        public void WriteString(string propertyName, string value)
        {
            WriteString(propertyName.AsSpan(), value.AsSpan());
        }

        public void WriteString(ReadOnlySpan<char> propertyName, string value)
        {
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, string value)
        {
            //Encoding.UTF8.GetBytes(value.AsSpan(), memory.Span);
        }

        public void WriteString(string propertyName, ReadOnlySpan<char> value)
        {
        }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<char> value)
        {
            Grow(1);

            var output = memory.Span;

            output[BytesPending++] = 0x02;

            WriteNullTerminatedString(propertyName);
            WriteStringValue(value);
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<char> value)
        {
        }

        public void WriteString(string propertyName, ReadOnlySpan<byte> value)
        {
        }

        public void WriteString(ReadOnlySpan<char> propertyName, ReadOnlySpan<byte> value)
        {
        }

        public void WriteString(ReadOnlySpan<byte> propertyName, ReadOnlySpan<byte> value)
        {
        }

        public void WriteStringValue(ReadOnlySpan<char> value)
        {
            Grow(value.Length * 3 + 5);

            var output = memory.Slice(BytesPending);

            //var length = Encoding.UTF8.GetBytes(value, output.Slice(4).Span) + 1;
            //BinaryPrimitives.WriteInt32LittleEndian(output.Span, length);

            //BytesPending += length + 4;
            memory.Span[BytesPending - 1] = 0x0;
        }

        private void WriteNullTerminatedString(ReadOnlySpan<char> value)
        {
            Grow(value.Length * 3 + 1);

            var output = memory.Slice(BytesPending);

            //var length = Encoding.UTF8.GetBytes(value, output.Span);

            //BytesPending += length;
            output.Span[BytesPending++] = 0x00;
        }

        private void Grow(int required)
        {
            if (memory.Length - BytesPending < required)
            {
                bufferWriter.Advance(BytesPending);

                BytesCommitted += BytesPending;
                BytesPending = 0;

                memory = bufferWriter.GetMemory(required);
            }
        }
    }
}
