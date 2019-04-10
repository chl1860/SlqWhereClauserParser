using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DParser.test
{
    [TestClass]
    public class MyTestClass
    {
        readonly Util util = null;
        public MyTestClass()
        {
            util = new Util();
        }

        [TestMethod]
        [Description("tests isMathExpr method")]
        public void TisMathExpr()
        {
            var str1 = "A='b'";
            var str2 = "A = 'bcd'";
            var str3 = "A= 'MMM'";
            var str4 = "A in('tt',ll)";
            var str5 = "A in ('tt',ll)";
            var str6 = "Ain('tt')";
            var str7 = "Alike'%b'";
            var str8 = "A like '%b'";

            Assert.AreEqual(util.IsMathExpr(str1), true);
            Assert.AreEqual(util.IsMathExpr(str2), true);
            Assert.AreEqual(util.IsMathExpr(str3), true);
            Assert.AreEqual(util.IsMathExpr(str4), true);
            Assert.AreEqual(util.IsMathExpr(str5), true);
            Assert.AreEqual(util.IsMathExpr(str6), false);
            Assert.AreEqual(util.IsMathExpr(str7), false);
            Assert.AreEqual(util.IsMathExpr(str8), true);

        }

        [TestMethod]
        [Description("it tests isLogicExpr method")]
        public void TisLogicExpr()
        {
            var str1 = "a and b";
            var str2 = "aand b";
            var str3 = "aandb";
            var str4 = "a or b";
            var str5 = "a orb";
            var str6 = "aorb";
            var str7 = "aor b";

            Assert.AreEqual(util.IsLogicExpr(str1), true);
            Assert.AreEqual(util.IsLogicExpr(str2), false);
            Assert.AreEqual(util.IsLogicExpr(str3), false);
            Assert.AreEqual(util.IsLogicExpr(str4), true);
            Assert.AreEqual(util.IsLogicExpr(str5), false);
            Assert.AreEqual(util.IsLogicExpr(str6), false);
            Assert.AreEqual(util.IsLogicExpr(str7), false);

        }
    }
}
