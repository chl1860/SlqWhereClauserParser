using CoreDParser;
using DeepEqual.Syntax;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace XUnitTest
{
    public class LexerTest
    {
        [Fact]
        [Description("it tests generate node list methods")]
        public void TGenerateNodeList()
        {
            var sqlStr = "FUNC_CODE = 'AAA' AND REGION_CODE = 'abc'";

            var lexer = new Lexer(sqlStr);
            var nodeList = lexer.GenerateNodeList(lexer.TokenArray);
            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var funcValNode = new ASTNode("Literal", "'AAA'", null, null, null);
            var logicNode = new ASTNode("LogicalExpr", "AND", null, null, null);
            var regNode = new ASTNode("Literal", "REGION_CODE", null, null, null);
            var regEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var reqValNode = new ASTNode("Literal", "'abc'", null, null, null);

            var expected = new List<ASTNode>
            {
                funcNode, funcEqNode, funcValNode, logicNode, regNode, regEqNode, reqValNode
            };

            nodeList.ShouldDeepEqual(expected);
        }

        [Fact]
        [Description("it tests generate ast node by node array with single node")]
        public void TGenerateSingleASTNode()
        {
            var lexer = new Lexer("");
            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var nodeList = new List<ASTNode> { funcNode };

            var ast = lexer.GenerateAstNode(nodeList);
            ast.ShouldDeepEqual(nodeList[0]);
        }

        [Fact]
        public void TestGenerateEmptyAstNode()
        {
            var nodeList = new List<ASTNode>();
            var lexer = new Lexer("");

            var ast = lexer.GenerateAstNode(nodeList);
            Assert.Null(ast);
        }

        [Fact]
        [Description("it tests  generate ast node methods")]
        public void TGenerateAstNodeMethod()
        {
            var str1 = "FUNC_CODE = 'AAA'";
            var lexer = new Lexer(str1);
            var nodeListist = lexer.GenerateNodeList(lexer.TokenArray);
            var ast = lexer.GenerateAstNode(nodeListist);

            var funcNode = new ASTNode("Literal", "FUNC_CODE", null, null, null);
            var funcEqNode = new ASTNode("MathExpr", "=", null, null, null);
            var funcValNode = new ASTNode("Literal", "'AAA'", null, null, null);
            funcEqNode.Left = funcNode;
            funcEqNode.Right = funcValNode;
            funcNode.Parent = funcEqNode;
            funcValNode.Parent = funcEqNode;

            Assert.Equal(funcEqNode.Left.Value, ast.Left.Value);
            Assert.Equal(funcEqNode.Right.Value, ast.Right.Value);
        }

        [Fact]
        public void TGenerateAstNodeMethodWithLogicKeyWords()
        {
            var str2 = "FUNC_CODE = 'BB' AND REGION_CODE = 'CC' OR TT IN ('MMMM', 'NNN')";
            var lexer = new Lexer(str2);
            var ast = lexer.GenerateAST();

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

            Assert.Equal(ast.Right.Right.Value, inNode.Right.Value);
        }

        [Fact]
        [Description("it tests split string by space")]
        public void TSplitStringBySpace()
        {
            var str1 = "Abc and \"CDE\" AND \"mn\"";
            var array = str1.Split(new char[] { ' ' });
            string[] expected = new string[] { "Abc", "and", "\"CDE\"", "AND", "\"mn\"" };

            array.ShouldDeepEqual(expected);
        }


        [Fact]
        [Description("It tests merged array method")]
        public void TGetMergedArray()
        {
            var str = "A and b and '(c, d, e)'";
            var str1 = "FUNC_CODE = 'aA'";
            var str2 = "(FUNC_CODE = 'aA')";

            var lexer = new Lexer(str);
            var lexer1 = new Lexer(str1);
            var lexer2 = new Lexer(str2);

            var result1 = lexer.GetMergedArray(str);
            var result2 = lexer1.GetMergedArray(str1);
            var result3 = lexer2.GetMergedArray(str2);

            var expected1 = new string[] { "A", "and", "b", "and", "'(c, d, e)'" };
            var expected2 = new string[] { "FUNC_CODE", "=", "'aA'" };


            result1.ShouldDeepEqual(expected1);
            result2.ShouldDeepEqual(expected2);
            result3.ShouldDeepEqual(expected2);

        }
    }
}
