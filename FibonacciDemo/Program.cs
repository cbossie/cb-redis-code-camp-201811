using System;
using System.Diagnostics;

namespace FibonacciDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the inefficient fibonacci number generator");

            string input = null;


            while (input != "quit")
            {
                Console.Write("Please enter which Fibonacci number you would like to calculate: ");
                input = Console.ReadLine();
                if (int.TryParse(input, out int seq))
                {
                    try
                    {

                        var s = Stopwatch.StartNew();
                        var result = LibFibonacci.FibonacciCalc.GetNthFibonacciString(seq);
                        var dur = TimeSpan.FromTicks(s.ElapsedTicks).TotalMilliseconds;
                        Console.WriteLine($"Fib[{seq}] = {result}");
                        Console.WriteLine($"Calculation duration (ms): {dur}");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("There was some sort of error, causing the earth to explode. Goodbye.");
                        return;
                    }

                }
            }

            Console.WriteLine("Press any Key to Continue");
            Console.ReadKey();




        }
    }
}
