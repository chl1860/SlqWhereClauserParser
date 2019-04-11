using CoreDParser;
using System.Collections.Generic;
using Xunit;

namespace XUnitTest
{
    public class ClsExtensionTest
    {
        [Fact]
        public void ReduceTest()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var result = list.Reduce((prev, curr) => prev + curr, null);

            Assert.Equal(15, result);
        }
    }
}
