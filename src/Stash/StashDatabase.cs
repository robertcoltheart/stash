using System;

namespace Stash
{
    /// <summary>
    /// Decisions:
    /// - 32-bit page numbers, allows for 16TB databases
    /// </summary>
    public class StashDatabase : IStashDatabase
    {
        public byte[] Get(byte[] key)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(byte[] key)
        {
        }

        public void Set(byte[] key, byte[] value)
        {
        }

        public void Get(byte[] key, out Span<byte> span)
        {
            span = Span<byte>.Empty;
        }
    }
}
