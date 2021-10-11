using System;

namespace Stash.Serialization
{
    public struct WriteStack
    {
        private int count;

        private WriteStackFrame[]? stack;

        public WriteStackFrame Current;

        public void Push()
        {
            count++;

            if (count > 1)
            {
                EnsureStack();

                stack![count - 2] = Current;

                Current = new WriteStackFrame();
            }
        }

        public void Pop()
        {
            count--;

            if (count > 0)
            {
                Current = stack![count - 1];
            }
        }

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
