using Stash.Serialization;
using Xunit;

namespace Stash.Tests.Serialization
{
    public class WriteStackTests
    {
        [Fact]
        public void CanPush()
        {
            var stack = new WriteStack();

            stack.Push();
            stack.Current.Position = 1;

            stack.Push();
            stack.Current.Position = 2;

            stack.Push();
            stack.Current.Position = 3;

            stack.Pop();
            stack.Pop();
            stack.Pop();

            Assert.Equal(1, stack.Current.Position);
        }
    }
}
