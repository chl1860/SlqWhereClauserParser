using Core;
using Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DParser
{
    public class Parser
    {
        ASTNode ast = null;
        List<SearchFilter> list = null;

        public Parser(string str)
        {
            var lexer = new Lexer(str);
            ast = lexer.GenerateAST();
            list = new List<SearchFilter>();
        }

        public SearchFilter GenerateFitlter(ASTNode root)
        {
            switch (root.Value.Trim().ToUpper())
            {
                case "=":
                case ">":
                case "<":
                case ">=":
                case "<=":
                    return GenerateMathSearchFilter(root);
                case "IN":
                    return GenerateInSearchFilter(root);
                case "LIKE":
                    return GenerateLikeSearchFilter(root);
            }
            return null;
        }

        public SearchFilter GenerateMathSearchFilter(ASTNode root)
        {
            var left = root.Left;
            var right = root.Right;
            var opDic = new Dictionary<string, string>
            {
                { "=", "eq" },
                { ">", "gt" },
                { ">=", "ge" },
                { "<", "lt" },
                { "<=", "le"}
            };

            return new SearchFilter
            {
                groupOp = root.Parent != null ? root.Parent.Value : "And",
                rules = new List<Filter>
                {
                    new Filter
                    {
                        data = right.Value.Replace("\"",""),
                        field = left.Value,
                        op = opDic[root.Value]
                    }
                }
            };

        }

        public SearchFilter GenerateInSearchFilter(ASTNode root)
        {
            var left = root.Left;
            var right = root.Right;
            var regx = new Regex(@"(?:\(|\)|\')", RegexOptions.IgnoreCase);

            var valList = regx.Replace(right.Value, "").Split(new char[] { ',' });
            var rules = new List<Filter>();

            foreach (var item in valList)
            {
                rules.Add(new Filter
                {
                    data = item.Trim(),
                    op = "eq",
                    field = left.Value
                });
            }

            return new SearchFilter
            {
                groupOp = "Or",
                rules = rules
            };
        }

        public SearchFilter GenerateLikeSearchFilter(ASTNode root)
        {
            var left = root.Left;
            var right = root.Right;
            var val = right.Value.Trim().Replace("'", "");
            string op = "";

            if(new Regex(@"^%.+%").IsMatch(val))
            {
                op = "cn";
            }else if(new Regex(@"^\%").IsMatch(val))
            {
                op = "ew";
            }else if(new Regex(@"\%$").IsMatch(val))
            {
                op = "sw";
            }

            return new SearchFilter
            {
                groupOp = root.Parent != null ? root.Parent.Value : "And",
                rules = new List<Filter>
                {
                    new Filter
                    {
                        data = new Regex(@"(?:^\s|\s$|\%|\')",RegexOptions.IgnoreCase).Replace(right.Value,""),
                        field = left.Value,
                        op=op
                    }
                }
            };
        }

        public void TraverseTreeToGenerateRules(ASTNode root)
        {
            if(root != null)
            {
                if(root.Type == "MathExpr")
                {
                    list.Add(GenerateFitlter(root));
                }
                else
                {
                    TraverseTreeToGenerateRules(root.Left);
                    TraverseTreeToGenerateRules(root.Right);
                }
            }
        }

        public ParserResult Parse()
        {
            TraverseTreeToGenerateRules(ast);
            //对 and 和 or 进行分类
            var andFilterList = this.list.Where(o => new Regex(@"\bAnd\b", RegexOptions.IgnoreCase).IsMatch(o.groupOp));
            var orFilterList = this.list.Where(o => new Regex(@"\bOr\b", RegexOptions.IgnoreCase).IsMatch(o.groupOp));
            var andRules = new List<Filter>();
            var orRules = new List<Filter>();

            if (andFilterList.Count() > 0)
            {
                foreach (var o in andFilterList)
                {
                    andRules.AddRange(o.rules);
                }

            }

            if (orFilterList.Count() > 0)
            {
                foreach (var o in orFilterList)
                {
                    orRules.AddRange(o.rules);
                }
            }

            //清理 1=1 的选项
            andRules = andRules.Where(o => !(o.data == "1" && o.op == "eq" && o.field == "1")).ToList();
            orRules = orRules.Where(o => !(o.data == "1" && o.op == "eq" && o.field == "1")).ToList();
            //orRules = orRules.filter(o => !(/^ 1$/ g.test(o.data) && o.op === 'eq' && /^ 1$/ g.test(o.field)));

            return new ParserResult
            {
                AndFilters = new SearchFilter
                {
                    groupOp = SearchFlag.AND.ToString(),
                    rules = andRules.Count() > 0 ? andRules : new List<Filter>()
                },
                OrFilters = new SearchFilter
                {
                    groupOp = SearchFlag.OR.ToString(),
                    rules = orRules.Count() > 0 ? orRules : new List<Filter>()
                }
            };
        }

        public List<SearchFilter> TraversedResult { get
            {
                return list;
            }
        }

        public ASTNode AstRoot
        {
            get { return ast; }
        }
    }
}
