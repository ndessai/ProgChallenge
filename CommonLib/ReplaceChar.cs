using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib
{
    public class ReplaceChar
    {
        //Return every possible combination where you replace the X with both 0 and 1
        //throws exception if there is any character other than 1, 0 or X or x (assumption here)
        public List<char[]> Compute(string input)
        {
            var prefixes = new List<char[]>();
            var defaultPrefix = new char[input.Length];
            prefixes.Add(defaultPrefix);

            int i = -1;
            foreach(var c in input)
            {
                i++;
                if (c == '1' || c == '0')
                {
                    //keep on copying the characters as is in all available combinations SO FAR
                    foreach(var prefix in prefixes)
                    {
                        prefix[i] = c;
                    }
                }
                else if (c == 'x' || c == 'X')
                {
                    //duplicate all combinations and on half add '1' and other half add '0' at the end
                    var newPrefixes = new List<char[]>();
                    foreach (var prefix in prefixes)
                    {
                        var newPrefix = new char[input.Length];
                        prefix.CopyTo(newPrefix, 0);
                        newPrefix[i] = '1';
                        newPrefixes.Add(newPrefix);
                        prefix[i] = '0';
                    }
                    prefixes.AddRange(newPrefixes);
                }
                else
                {
                    throw new ArgumentException(
                        string.Format("input contains invalid character {0} at {1}", c, i),
                        "input");
                }
            }
            return prefixes; //prefix is list of all strings after parsing the entire input string
        }
    }
}
