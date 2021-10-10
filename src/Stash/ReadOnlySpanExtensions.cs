using System;
using System.Text;

namespace Stash
{
    public static class ReadOnlySpanExtensions
    {
        public static string GetString(this ref ReadOnlySpan<byte> value)
        {
#if NETSTANDARD || NETFRAMEWORK
            var bytes = value.ToArray();
#else
            var bytes = value;
#endif

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
