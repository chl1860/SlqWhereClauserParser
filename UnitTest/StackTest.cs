﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DParser.test
{
    [TestClass]
    public class StackTest
    {
        readonly SimpleStack<int> stack = null; 
        public StackTest()
        {
            stack = new SimpleStack<int>();   
        }

        [TestMethod]
        [Description("it tests stack push method")]
        public void TPush()
        {
            stack.Push(1);
            stack.Push(2);

            var len = stack.Len;

            Assert.AreEqual(2, len);
        }

        [TestMethod]
        [Description("it tests stack pop method")]
        public void TPop()
        {
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            var item = stack.Pop();
            Assert.AreEqual(3, item);
        }

        [TestMethod]
        [Description("tests stack clear method")]
        public void TClear()
        {
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);

            stack.Clear();
            Assert.AreEqual(0, stack.Len);
        }
    }
}
