using CoreDParser;
using System.ComponentModel;
using Xunit;

namespace XUnitTest
{
    public class StackTest
    {
        readonly SimpleStack<int> stack = null;
        public StackTest()
        {
            stack = new SimpleStack<int>();
        }

        [Fact]
        [Description("it tests stack push method")]
        public void TPush()
        {
            stack.Push(1);
            stack.Push(2);

            var len = stack.Len;

            Assert.Equal(2, len);
        }

        [Fact]
        [Description("it tests stack pop method")]
        public void TPop()
        {
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var item = stack.Pop();
            Assert.Equal(3, item);
        }

        [Fact]
        [Description("tests stack clear method")]
        public void TClear()
        {
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            stack.Clear();
            Assert.Equal(0, stack.Len);
        }
    }
}
