using System;
using System.Text;

namespace Stash
{
    internal static class EncodingExtensions
    {
#if NETSTANDARD
        public static unsafe int GetBytes(this Encoding encoding, ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            fixed (char* charsRef = &chars.GetPinnableReference())
            fixed (byte* bytesRef = &bytes.GetPinnableReference())
            {
                return encoding.GetBytes(charsRef, chars.Length, bytesRef, bytes.Length);
            }
        }
#endif
    }
}
