using System;

namespace Stash.Serialization
{
    public struct WriteStackFrame
    {
        public int Depth;

        public int Position;

        public Memory<byte> LengthMemory;
    }
}
