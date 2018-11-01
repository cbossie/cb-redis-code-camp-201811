using System;
using System.Numerics;

namespace LibFibonacci
{
    public class FibonacciCalc
    {
        public static BigInteger CalcFibonacci(BigInteger num)
        {

            if (num < 0)
            {
                throw new InvalidOperationException();
            }
            if (num == 0)
            {
                return 0;
            }

            if (num == 1)
            {
                return 1;
            }

            return CalcFibonacci(num - 1) + CalcFibonacci(num - 2);
        }

        public static string GetNthFibonacciString(int n)
        {
            var fibo = CalcFibonacci(n);
            return fibo.ToString();
        }
    }
}
