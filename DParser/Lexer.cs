using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DParser
{
    public class Lexer
    {
        List<string> tokenArray = null;
        Util util = null;
        Tokenizer tokenizer = null;
        public Lexer(string str)
        {
            tokenizer = new Tokenizer();
            tokenArray = tokenizer.GetMergedArray(str);
            util = new Util();
        }

        public List<string> TokenArray
        {
            get { return this.tokenArray; }
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
                            list.AddRange(GenerateNodeList(tokenizer.GetMergedArray(o)));
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
            var nodeListist = GenerateNodeList(this.tokenArray);
            return GenerateAstNode(nodeListist);
        }
    }
}
