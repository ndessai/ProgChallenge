using CommonLib;
using System;
using System.Collections.Generic;
using System.IO;

namespace FindPair
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length < 2)
            {
                Console.WriteLine("Usage: FindPair <filename> <giftcardlimit>\n" +
                    "eg: FindPair prices.txt 2500");
                Console.Read();
                return;
            }
            if(!File.Exists(args[0]))
            {
                Console.WriteLine("Error: " + args[0] + " file does not exist");
                return;
            }
            uint limit = 0;
            if(!UInt32.TryParse(args[1], out limit))
            {
                Console.WriteLine(args[1] + " is not a valid uint");
                return;
            }
            uint friends = 2;
            if(args.Length == 3)
            {
                if(!UInt32.TryParse(args[2], out friends))
                {
                    Console.WriteLine(args[2] + " is not a valid uint");
                    return;
                }
            }
            var lines = new List<string>();
            using(var reader = File.OpenText(args[0]))
            {
                var line = reader.ReadLine();
                while(!string.IsNullOrWhiteSpace(line))
                {
                    lines.Add(line);
                    line = reader.ReadLine();
                }
            }
            var optimalPairs = new OptimalPairs();
            try
            {
                var items = optimalPairs.Compute(lines, limit, friends);
                foreach(var item in items)
                {
                    Console.Write(item.Name + " " + item.Price + "     ");
                }
                if(items.Count <= 0)
                {
                    Console.WriteLine("Not Possible");
                }
            }
            catch(ApplicationException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();
        }
    }
}
