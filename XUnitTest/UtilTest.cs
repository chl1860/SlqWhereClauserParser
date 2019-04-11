using CoreDParser;
using System.ComponentModel;
using Xunit;

namespace XUnitTest
{
    public class UtilTest
    {
        private readonly Util util = null;
        public UtilTest()
        {
            util = new Util();
        }

        [Fact]
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

            Assert.True(util.IsMathExpr(str1));
            Assert.True(util.IsMathExpr(str2));
            Assert.True(util.IsMathExpr(str3));
            Assert.True(util.IsMathExpr(str4));
            Assert.True(util.IsMathExpr(str5));
            Assert.False(util.IsMathExpr(str6));
            Assert.False(util.IsMathExpr(str7));
            Assert.True(util.IsMathExpr(str8));

        }

        [Fact]
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

            Assert.True(util.IsLogicExpr(str1));
            Assert.False(util.IsLogicExpr(str2));
            Assert.False(util.IsLogicExpr(str3));
            Assert.True(util.IsLogicExpr(str4));
            Assert.False(util.IsLogicExpr(str5));
            Assert.False(util.IsLogicExpr(str6));
            Assert.False(util.IsLogicExpr(str7));

        }

        [Fact]
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

            Assert.True(result1);
            Assert.True(result2);
            Assert.False(result3);
            Assert.True(result4);
            Assert.False(result5);
            Assert.False(result6);
            Assert.False(result7);
            Assert.True(result8);
        }
    }
}
