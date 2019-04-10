using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DParser
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

        public T Reduce<T, U>(Func<U, T, T> func, IEnumerable<U> list, T acc)
        {
            foreach (var i in list)
                acc = func(i, acc);

            return acc;
        }
    }
}
