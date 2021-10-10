using System;
using System.Buffers;

namespace Stash
{
    internal class PooledByteWriterBuffer : IBufferWriter<byte>, IDisposable
    {
        private const int DefaultBufferSize = 1024 * 16;

        private byte[] buffer;

        private int index;

        private bool disposed;

        public PooledByteWriterBuffer(int capacity = DefaultBufferSize)
        {
            buffer = ArrayPool<byte>.Shared.Rent(capacity);
        }

        public void Clear()
        {
            buffer.AsSpan(0, index).Clear();
            index = 0;
        }

        public void Advance(int count)
        {
            index += count;
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            ResizeBuffer(sizeHint);

            return buffer.AsMemory(index);
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            ResizeBuffer(sizeHint);

            return buffer.AsSpan();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            Clear();

            var data = buffer;

            buffer = null!;
            disposed = true;

            ArrayPool<byte>.Shared.Return(data);
        }

        private void ResizeBuffer(int sizeHint)
        {
            var available = buffer.Length - index;

            if (sizeHint > available)
            {
                var growLength = Math.Max(sizeHint, buffer.Length);
                var totalLength = buffer.Length + growLength;

                if ((uint)totalLength > int.MaxValue)
                {
                    throw new OutOfMemoryException("Writer buffer exceeded maximum length");
                }

                var oldBuffer = buffer;

                buffer = ArrayPool<byte>.Shared.Rent(totalLength);

                var data = oldBuffer.AsSpan(0, index);
                data.CopyTo(buffer);
                data.Clear();

                ArrayPool<byte>.Shared.Return(oldBuffer);
            }
        }
    }
}
