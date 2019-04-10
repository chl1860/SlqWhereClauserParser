using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DParser
{
    public class Lexer
    {
        string sqlStr = null;
        Util util = null;
        public Lexer(string str)
        {
            sqlStr = str;
            util = new Util();
        }

        public List<string> TokenArray
        {
            get { return this.GetMergedArray(this.sqlStr); }
        }

        public ASTNode GenerateAstNode(List<ASTNode> nodeList)
        {
            var len = nodeList.Count();
            if (len == 0)
            {
                return null;
            }
            else if (len == 1)
            {
                return nodeList[len - 1];
            }
            else
            {
                return nodeList.Reduce((prev, curr) =>
                {
                    if (curr.Type == "MathExpr")
                    {
                        if (prev.Type == "Literal")
                        {
                            curr.Left = prev;
                            prev.Parent = curr;
                            return curr;
                        }
                        else if (prev.Type == "LogicalExpr")
                        {
                            curr.Left = prev.Right;
                            prev.Right.Parent = curr;
                            prev.Right = curr;
                            curr.Parent = prev;

                            return prev;
                        }
                        return curr;

                    }
                    else if (curr.Type == "LogicalExpr")
                    {
                        curr.Left = prev;
                        prev.Parent = curr;

                        return curr;
                    }
                    else //if (curr.Type == "Literal")
                    {
                        if (prev.Type == "LogicalExpr" || prev.Type == "MathExpr")
                        {
                            if (prev.Right == null)
                            {
                                prev.Right = curr;
                                curr.Parent = prev;
                            }
                            else
                            {
                                var p = prev;
                                ASTNode temp = null;
                                while (p != null)
                                {
                                    temp = p;
                                    p = p.Right;
                                }
                                temp.Right = curr;
                                curr.Parent = temp;
                            }
                            return prev;
                        }
                        return curr;
                    }
                }, null);
            }
        }

        public List<ASTNode> GenerateNodeList(List<string> array)
        {
            var list = new List<ASTNode>();
            array.ForEach(o =>
            {
                if (util.IsMathExpr(o) && !o.StartsWith("("))
                {
                    list.Add(new ASTNode("MathExpr", o, null, null, null));

                }
                else if (util.IsLogicExpr(o) && !o.StartsWith("("))
                {
                    list.Add(new ASTNode("LogicalExpr", o, null, null, null));
                }
                else
                {
                    if (o.StartsWith("("))
                    {
                        var lastItem = list.LastOrDefault();
                        if (lastItem == null ||!(lastItem.Type == "MathExpr" && new Regex(@"\bin\b", RegexOptions.IgnoreCase).IsMatch(lastItem.Value))) {
                            list.AddRange(GenerateNodeList(this.GetMergedArray(o)));
                            list.Reverse();
                        }
                        else
                        {
                            list.Add(new ASTNode("Literal", o, null, null, null));
                        }

                    }
                    else
                    {
                        list.Add(new ASTNode("Literal", o, null, null, null));
                    }
                }
            });

            return list;
        }

        public ASTNode GenerateAST()
        {
            var nodeListist = GenerateNodeList(this.GetMergedArray(this.sqlStr));
            return GenerateAstNode(nodeListist);
        }

        public List<string> GetMergedArray(string str)
        {
            var array = str.Split(new char[] { ' ' });
            var len = array.Length;
            List<string> result = new List<string>(array.Length);

            int i = 0, j = i - 1;
            while (i < len && j < len)
            {
                if (j != -1 && result[j] != null && !util.IsFullString(result[j]))
                {
                    do
                    {
                        result[j] = string.Format("{0} {1}", result[j], array[i]);
                        i++;
                    } while (!util.IsFullString(result[j]) && i < len);
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
            if (!string.IsNullOrEmpty(str) && result[0] == str)
            {
                str = (new Regex(@"\(([^\)]+)\)")).Replace(str, "$1");
                return GetMergedArray(str);
            }
            return result;
        }

    }
}
