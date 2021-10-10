using System;

namespace Stash.Serialization
{
    public struct WriteStack
    {
        private int count;

        private WriteStackFrame[]? stack;

        public WriteStackFrame Current;

        private void EnsureStack()
        {
            if (stack is null)
            {
                stack = new WriteStackFrame[4];
            }
            else if (count - 1 == stack.Length)
            {
                Array.Resize(ref stack, 2 * stack.Length);
            }
        }
    }
}
