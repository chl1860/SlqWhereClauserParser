using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DParser.test
{
    [TestClass]
    public class TokenTest
    {
        readonly Tokenizer tokenizer = null;
        public TokenTest()
        {
            tokenizer = new Tokenizer();
        }

        [TestMethod]
        [Description("it tests split string by space")]
        public void TSplitStringBySpace()
        {
            var str1 = "Abc and \"CDE\" AND \"mn\"";
            var array = str1.Split(new char[] { ' ' });
            string[] expected = new string[] { "Abc", "and", "\"CDE\"", "AND", "\"mn\"" };

            array.ShouldDeepEqual(expected);
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


            var result1 = tokenizer.IsFullString(str1);
            var result2 = tokenizer.IsFullString(str2);
            var result3 = tokenizer.IsFullString(str3);
            var result4 = tokenizer.IsFullString(str4);
            var result5 = tokenizer.IsFullString(str5);
            var result6 = tokenizer.IsFullString(str6);
            var result7 = tokenizer.IsFullString(str7);
            var result8 = tokenizer.IsFullString(str8);

            Assert.IsTrue(result1);
            Assert.IsTrue(result2);
            Assert.IsFalse(result3);
            Assert.IsTrue(result4);
            Assert.IsFalse(result5);
            Assert.IsFalse(result6);
            Assert.IsFalse(result7);
            Assert.IsTrue(result8);
        }

        [TestMethod]
        [Description("It tests merged array method")]
        public void TGetMergedArray()
        {
            var str = "A and b and '(c, d, e)'";
            var str1 = "FUNC_CODE = 'aA'";
            var str2 = "(FUNC_CODE = 'aA')";

            var result1 = tokenizer.GetMergedArray(str);
            var result2 = tokenizer.GetMergedArray(str1);
            var result3 = tokenizer.GetMergedArray(str2);

            var expected1 = new string[] { "A", "and", "b", "and", "'(c, d, e)'" };
            var expected2 = new string[] { "FUNC_CODE", "=", "'aA'" };
           

            result1.ShouldDeepEqual(expected1);
            result2.ShouldDeepEqual(expected2);
            result3.ShouldDeepEqual(expected2);

        }
    }
}
