using CommonLib;
using System;

namespace ReplaceX
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ReplaceX <string with 1 0 and X>\n" +
                    "eg: FindPair 0X");
                Console.Read();
                return;
            }
            var charReplace = new ReplaceChar();
            try
            {
                var items = charReplace.Compute(args[0]);
                foreach (var item in items)
                {
                    Console.WriteLine(item);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();
        }
    }
}
