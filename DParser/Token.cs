using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DParser
{
    public class Tokenizer
    {
        public Tokenizer()
        {

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

        public List<string> GetMergedArray(string str)
        {
            var array = str.Split(new char[] { ' ' });
            var len = array.Length;
            List<string> result = new List<string>(array.Length);

            int i = 0, j = i - 1;
            while(i < len && j < len)
            {
                if (j != -1 && result[j] != null && !this.IsFullString(result[j]))
                {
                    do
                    {
                        result[j] = string.Format("{0} {1}", result[j], array[i]);
                        i++;
                    } while (!this.IsFullString(result[j]) && i < len);
                }
                else
                {
                    result.Add(array[i]);
                    i++;
                    j++;
                }
            }

            result.ForEach(o => o.Trim());
            
            //排除：(FUNC_CODE = 'aa') 这一类的情况
            if(!string.IsNullOrEmpty(str) && result[0] == str)
            {
                str = (new Regex(@"\(([^\)]+)\)")).Replace(str, "$1");
                return GetMergedArray(str);
            }
            return result;
        }
    }
}
