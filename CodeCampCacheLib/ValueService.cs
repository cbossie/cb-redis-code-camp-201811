using System;
using System.Collections.Generic;
using System.Text;
using LibFibonacci;

namespace CodeCampCacheLib
{
    public class ValueService : IValueService
    {
        
        public virtual string GetNthFibonacci(int n)
        {
            return FibonacciCalc.GetNthFibonacciString(n);
        }
    }
}
