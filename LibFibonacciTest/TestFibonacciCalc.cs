using System;
using LibFibonacci;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LibFibonacciTest
{
    [TestClass]
    public class TestFibonacciCalc
    {
        [TestMethod]
        public void TestFibonacciCalcBaseCase()
        {
            var i = 1;
            var f = FibonacciCalc.GetNthFibonacciString(i);
            Assert.AreEqual(f, "1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestFibonacciCalcNegative()
        {
            var i = -1;
            var f = FibonacciCalc.GetNthFibonacciString(i);
        }

        [TestMethod]
        public void TestFibonacciCalcZero()
        {
            var i = 0;
            var f = FibonacciCalc.GetNthFibonacciString(i);
            Assert.AreEqual(f, "0");
        }


        [TestMethod]
        public void TestFibonacciCalcLarger()
        {
            var i = 44;
            var f = FibonacciCalc.GetNthFibonacciString(i);
            Assert.AreEqual(f, "701408733 ");
        }

    }
}
