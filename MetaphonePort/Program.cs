using System;
using System.Diagnostics;
using System.Linq;

namespace MetaphonePort
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            foreach(var abc in Enumerable.Range(1, 100000).Select(x => Metaphone.Encode("Overby")).Distinct())
            {
                Console.WriteLine(abc);
            }

            Console.WriteLine(sw.Elapsed);
            Console.ReadKey();
        }
    }
}
