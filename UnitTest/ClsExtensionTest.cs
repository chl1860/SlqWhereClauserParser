using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DParser.test
{
    [TestClass]
    public class ClsExtensionTest
    {
        [TestMethod]
        public void ReduceTest()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };
            var result = list.Reduce((prev, curr) => prev + curr,null);

            Assert.AreEqual(15, result);
        }
    }
}
