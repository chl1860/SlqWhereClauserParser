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

        [TestMethod]
        [Description("it tests splited item is full string")]
        public void TIsFullString()
        {
            string str1 = "Abc",
                   str2 = "Abc'abc'",
                   str3 = "Abc'abc",
                   str4 = "('ab', 'd')",
                   str5 = "('ab', 'd'",
                   str6 = "'(ab",
                   str7 = "'(abc)",
                   str8 = "'(abc)'";


            var result1 = util.IsFullString(str1);
            var result2 = util.IsFullString(str2);
            var result3 = util.IsFullString(str3);
            var result4 = util.IsFullString(str4);
            var result5 = util.IsFullString(str5);
            var result6 = util.IsFullString(str6);
            var result7 = util.IsFullString(str7);
            var result8 = util.IsFullString(str8);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsTrue(result4);
            Assert.IsFalse(result5);
            Assert.IsFalse(result6);
            Assert.IsFalse(result7);
            Assert.IsTrue(result8);
        }

    }
}
