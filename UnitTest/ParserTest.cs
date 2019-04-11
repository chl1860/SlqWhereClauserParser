using DeepEqual.Syntax;
using DParser.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DParser.test
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        [Description("")]
        public void TGeneraInSearchFilter()
        {
            var inFieldNode = new ASTNode("Literal", "TT", null, null, null);
            var inValNode = new ASTNode("Literal", "('MMMM', 'Tsss')", null, null, null);
            var inNode = new ASTNode("MathExpr", "IN", null, null, null);

            inNode.Left = inFieldNode;
            inFieldNode.Parent = inNode;

            inNode.Right = inValNode;
            inValNode.Parent = inNode;

            var parser = new Parser("");
            var result = parser.GenerateInSearchFilter(inNode);
            var expected = new SearchFilter
            {
                groupOp = "Or",
                rules = new List<Filter>
                {
                    new Filter { data= "MMMM", op= "eq", field= "TT" },
                    new Filter { data= "Tsss", op= "eq", field= "TT" },
                }
            };

            result.IsDeepEqual(expected);
        }

        [TestMethod]
        [Description("it tests GenerateLikeSearchFilter method")]
        public void TGenerateLikeSearchFilter()
        {
            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "like", null, null, null);
            var funcValNode = new ASTNode("Literal", "'%BB%'", null, null, null);
            funcEqNode.Left = funcNode;
            funcEqNode.Right = funcValNode;
            funcNode.Parent = funcEqNode;
            funcValNode.Parent = funcEqNode;

            var funcNode1 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode1 = new ASTNode("MathExpr", "like", null, null, null);
            var funcValNode1 = new ASTNode("Literal", "'%BB'", null, null, null);
            funcEqNode1.Left = funcNode1;
            funcEqNode1.Right = funcValNode1;
            funcNode1.Parent = funcEqNode1;
            funcValNode1.Parent = funcEqNode1;

            var funcNode2 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode2 = new ASTNode("MathExpr", "like", null, null, null);
            var funcValNode2 = new ASTNode("Literal", "'BB%'", null, null, null);
            funcEqNode2.Left = funcNode2;
            funcEqNode2.Right = funcValNode2;
            funcNode2.Parent = funcEqNode2;
            funcValNode2.Parent = funcEqNode2;

            var parser = new Parser("");
            var actual1 = parser.GenerateLikeSearchFilter(funcEqNode);
            var actual2 = parser.GenerateLikeSearchFilter(funcEqNode1);
            var actual3 = parser.GenerateLikeSearchFilter(funcEqNode2);

            actual1.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{data= "BB", op= "cn", field= "FUNC_CODE" }
                }
            });

            actual2.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{data= "BB", op= "ew", field= "FUNC_CODE" }
                }
            });

            actual3.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{data = "BB", op = "sw", field="FUNC_CODE" }
                }
            });
        }

        [TestMethod]
        public void TGenerateFilter()
        {
            var parser = new Parser("");

            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var funcValNode = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode.Left = funcNode;
            funcEqNode.Right = funcValNode;
            funcNode.Parent = funcEqNode;
            funcValNode.Parent = funcEqNode;

            var actual1 = parser.GenerateFitlter(funcEqNode);
            actual1.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{data = "BB",op = "eq",field = "FUNC_CODE"}
                }
            });

            var funcNode2 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode2 = new ASTNode("MathExpr", ">", null, null, null);
            var funcValNode2 = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode2.Left = funcNode2;
            funcEqNode2.Right = funcValNode2;
            funcNode2.Parent = funcEqNode2;
            funcValNode2.Parent = funcEqNode2;

            var actual2 = parser.GenerateFitlter(funcEqNode2);
            actual2.IsDeepEqual(
                new SearchFilter
                {
                    groupOp = "And",
                    rules = new List<Filter>
                    {
                        new Filter{ data= "BB",op= "gt",field= "FUNC_CODE"}
                    }
                }
            );

            var funcNode3 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode3 = new ASTNode("MathExpr", "<", null, null, null);
            var funcValNode3 = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode3.Left = funcNode3;
            funcEqNode3.Right = funcValNode3;
            funcNode3.Parent = funcEqNode3;
            funcValNode2.Parent = funcEqNode3;

            var actual3 = parser.GenerateFitlter(funcEqNode3);
            actual3.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter
                    {
                        data= "BB",
                        op = "lt",
                        field= "FUNC_CODE"
                    }
                }
            });

            var funcNode4 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode4 = new ASTNode("MathExpr", ">=", null, null, null);
            var funcValNode4 = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode4.Left = funcNode4;
            funcEqNode4.Right = funcValNode4;
            funcNode4.Parent = funcEqNode4;
            funcValNode4.Parent = funcEqNode4;

            var actual4 = parser.GenerateFitlter(funcEqNode4);
            actual4.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter
                    {
                        data= "BB",
                        op = "ge",
                        field= "FUNC_CODE"
                    }
                }
            });

            var funcNode5 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode5 = new ASTNode("MathExpr", "<=", null, null, null);
            var funcValNode5 = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode5.Left = funcNode5;
            funcEqNode5.Right = funcValNode5;
            funcNode5.Parent = funcEqNode5;
            funcValNode5.Parent = funcEqNode5;

            var actual5 = parser.GenerateFitlter(funcEqNode5);
            actual5.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter
                    {
                        data= "BB",
                        op = "le",
                        field= "FUNC_CODE"
                    }
                }
            });

            //like
            var funcNode6 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode6 = new ASTNode("MathExpr", "like", null, null, null);
            var funcValNode6 = new ASTNode("Literal", "'%BB%'", null, null, null);
            funcEqNode6.Left = funcNode6;
            funcEqNode6.Right = funcValNode6;
            funcNode6.Parent = funcEqNode6;
            funcValNode6.Parent = funcEqNode6;

            var funcNode7 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode7 = new ASTNode("MathExpr", "like", null, null, null);
            var funcValNode7 = new ASTNode("Literal", "'%BB'", null, null, null);
            funcEqNode7.Left = funcNode7;
            funcEqNode7.Right = funcValNode7;
            funcNode7.Parent = funcEqNode7;
            funcValNode7.Parent = funcEqNode7;

            var funcNode8 = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode8 = new ASTNode("MathExpr", "like", null, null, null);
            var funcValNode8 = new ASTNode("Literal", "'BB%'", null, null, null);
            funcEqNode8.Left = funcNode8;
            funcEqNode8.Right = funcValNode8;
            funcNode8.Parent = funcEqNode8;
            funcValNode8.Parent = funcEqNode8;

            var actual6 = parser.GenerateFitlter(funcEqNode6);
            actual6.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{ data= "BB", op = "cn", field = "FUNC_CODE" }
                }
            });

            var actual7 = parser.GenerateFitlter(funcEqNode7);
            actual7.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{ data= "BB", op = "ew", field = "FUNC_CODE" }
                }
            });

            var actual8 = parser.GenerateFitlter(funcEqNode8);
            actual8.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{ data= "BB", op = "sw", field = "FUNC_CODE" }
                }
            });
        }

        [TestMethod]
        [Description("It tests TraverseTreeToGenerateRules method if condition")]
        public void TTraverseTreeToGenerateRules1()
        {
            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var funcValNode = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode.Left = funcNode;
            funcEqNode.Right = funcValNode;
            funcNode.Parent = funcEqNode;
            funcValNode.Parent = funcEqNode;

            var parser = new Parser("");

            parser.TraverseTreeToGenerateRules(funcEqNode);
            var actual1 = parser.TraversedResult;

            actual1.IsDeepEqual(new SearchFilter
            {
                groupOp = "And",
                rules = new List<Filter>
                {
                    new Filter{data="BB",op="eq",field="FUNC_CODE"}
                }
            });
        }

        [TestMethod]
        [Description("It tests TraverseTreeToGenerateRules method else condition")]
        public void TTraverseTreeToGenerateRules2()
        {
            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var funcValNode = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode.Left = funcNode;
            funcEqNode.Right = funcValNode;
            funcNode.Parent = funcEqNode;
            funcValNode.Parent = funcEqNode;

            var regNode = new ASTNode("Literal", "REGION_CODE", null, null, null);
            var regEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var regValNode = new ASTNode("Literal", "'CC'", null, null, null);
            regEqNode.Left = regNode;
            regEqNode.Right = regValNode;
            regNode.Parent = regEqNode;
            regValNode.Parent = regEqNode;

            var andNode = new ASTNode("LogicalExpr", "AND", funcEqNode, regEqNode, null);
            funcEqNode.Parent = andNode;
            regEqNode.Parent = andNode;

            var parser = new Parser("");

            parser.TraverseTreeToGenerateRules(andNode);
            parser.TraversedResult.IsDeepEqual(new List<SearchFilter>
            {
                new SearchFilter
                {
                    groupOp="AND",
                    rules = new List<Filter>
                    {
                        new Filter{data="BB",op="eq",field="FUNC_CODE"}
                    }
                },
                new SearchFilter
                {
                    groupOp="AND",
                    rules = new List<Filter>
                    {
                        new Filter{data="CC",op="eq",field="REGION_CODE"}
                    }
                }

            });
        }

        [TestMethod]
        [Description("It tests parse string to ast")]
        public void TParrseAstRoot()
        {
            var parser = new Parser("FUNC_CODE = 'BB' AND REGION_CODE = 'CC' OR TT IN ('MMMM', 'NNN')");

            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var funcValNode = new ASTNode("Literal", "'BB'", null, null, null);
            funcEqNode.Left = funcNode;
            funcEqNode.Right = funcValNode;
            funcNode.Parent = funcEqNode;
            funcValNode.Parent = funcEqNode;

            var regNode = new ASTNode("Literal", "REGION_CODE", null, null, null);
            var regEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var regValNode = new ASTNode("Literal", "'CC'", null, null, null);
            regEqNode.Left = regNode;
            regEqNode.Right = regValNode;
            regNode.Parent = regEqNode;
            regValNode.Parent = regEqNode;

            var andNode = new ASTNode("LogicalExpr", "AND", funcEqNode, regEqNode, null);
            funcEqNode.Parent = andNode;
            regEqNode.Parent = andNode;

            var inFieldNode = new ASTNode("Literal", "TT", null, null, null);
            var inValNode = new ASTNode("Literal", "('MMMM', 'NNN')", null, null, null);
            var inNode = new ASTNode("MathExpr", "IN", null, null, null);
            var orNode = new ASTNode("LogicalExpr", "OR", null, null, null);
            orNode.Left = andNode;
            orNode.Right = inNode;
            andNode.Parent = orNode;

            inNode.Parent = orNode;
            inNode.Left = inFieldNode;
            inFieldNode.Parent = inNode;

            inNode.Right = inValNode;
            inValNode.Parent = inNode;

            Assert.AreEqual(parser.AstRoot.Right.Value,orNode.Right.Value);

        }

        [TestMethod]
        [Description("It tests parse method")]
        public void TParse()
        {
            var parser = new Parser("1 = 1 and FUNC_CODE = 'BB' AND REGION_CODE = 'CC' OR TT IN ('MMMM', 'NNN')");
            var obj = parser.Parse();
            var len = obj.AndFilters.rules.Count;
            var len1 = obj.OrFilters.rules.Count;

            Assert.AreEqual(2, len);
            Assert.AreEqual(2, len1);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var parser = new Parser("(FUNC_CODE = 'BB')");
            var obj = parser.Parse();
            var len = obj.AndFilters.rules.Count;

            Assert.AreEqual(1, len);
        }

        [TestMethod]
        public void MyMethod2()
        {
            var parser = new Parser("(A = 'a' AND (B = 'b' OR C = 'c'))");
            var obj = parser.Parse();
            var len = obj.AndFilters.rules.Count;

            Assert.AreEqual(1, len);
        }

        [TestMethod]
        public void MyMethod3()
        {
            var parser = new Parser("(A = 'a' AND B = 'b') AND (B = 'b' OR C = 'c')");
            var obj = parser.Parse();
            var len = obj.AndFilters.rules.Count;

            Assert.AreEqual(2, len);
        }

        [TestMethod]
        public void MyMethod4()
        {
            var parser = new Parser("(A = 'a' AND B = 'b') AND (B = 'b' OR C = 'c' AND (D = 'D' OR E = 'e'))");
            var obj = parser.Parse();
            var len = obj.AndFilters.rules.Count;

            Assert.AreEqual(3, len);
        }
    }
}
