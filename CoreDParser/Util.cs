using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoreDParser
{
    public class Util
    {
        public Util()
        {

        }

        public bool IsMathExpr(string str)
        {
            var mathTokens = new List<Regex>
            {
                new Regex(@"=",RegexOptions.IgnoreCase),
                new Regex(@"\b(?:In)\b",RegexOptions.IgnoreCase),
                new Regex(@"\blike\b",RegexOptions.IgnoreCase)
            };

            return mathTokens.Where(o => o.IsMatch(str)).Count() > 0;
        }

        public bool IsLogicExpr(string str)
        {
            var logicTokens = new List<Regex>
            {
                new Regex(@"\bAnd\b",RegexOptions.IgnoreCase),
                new Regex(@"\bOr\b",RegexOptions.IgnoreCase)
            };

            return logicTokens.Where(o => o.IsMatch(str)).Count() > 0;
        }

        public bool IsFullString(string str)
        {
            //不包含  '' 的字符串
            if (str.IndexOf("'") == -1 && str.IndexOf("(") == -1)
            {
                return true;
            }

            var index = 0;
            Regex checker = null;
            Regex sigleQuoteRegx = new Regex(@"\'");
            Regex braceRegx = new Regex(@"(?:\(|\))");
            var singleQuoteIndex = str.IndexOf("'");
            var braceIndex = str.IndexOf("(");

            if (singleQuoteIndex != -1 && braceIndex != -1)
            { //当单引号和括号同时存在时，谁先出现以谁为准
                if (singleQuoteIndex < braceIndex)
                {
                    checker = sigleQuoteRegx;
                }
                else
                {
                    checker = braceRegx;
                }
            }
            else if (str.IndexOf("(") != -1)
            {
                index = str.IndexOf("(");
                checker = braceRegx;
            }
            else
            {
                index = str.IndexOf("'");
                checker = sigleQuoteRegx;
            }

            var subStr = str.Substring(index);
            var stack = new SimpleStack<string>();

            foreach (char ch in subStr)
            {
                if (checker != null)
                {
                    if (checker.IsMatch(ch.ToString()))
                    {
                        stack.Clear();
                    }
                    else
                    {
                        stack.Push(ch.ToString());
                    }
                }
            }

            return stack.Len == 0;
        }


        public T Reduce<T, U>(Func<U, T, T> func, IEnumerable<U> list, T acc)
        {
            foreach (var i in list)
                acc = func(i, acc);

            return acc;
        }
    }
}
