using System;
using System.Collections.Generic;
using System.IO;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Part1(args);
            Part2(args);
        }

        static void Part1(string[] args) {
            var alllines = File.ReadAllLines(args[0]);

            int bits = alllines[0].Length;

            int gamma = 0;
            int epsilon = 0;

            for (int i = 0; i < bits; i++) {
                gamma = gamma * 2;
                epsilon = epsilon * 2;

                int zeros = 0;
                int ones = 0;
                for(int j = 0; j < alllines.Length; j++) {
                    if (alllines[j][i] == '1')
                        ones++;
                    else
                        zeros++;
                }
                if (zeros > ones)
                    epsilon += 1;
                else
                    gamma += 1;
            }
            Console.Out.WriteLine($"Epsilon = {epsilon}, Gamma = {gamma}");
            Console.Out.WriteLine($"Result is {gamma * epsilon}");
        }

        static void Part2(string[] args) {
            var alllines = File.ReadAllLines(args[0]);

            int bits = alllines[0].Length;
            
            var oxygen = Part2_Gen(new List<string>(alllines), bits, (s,c,zeros,ones) => (
                (ones >= zeros && s[c] == '1') || (ones < zeros && s[c] == '0')
            ));
            var scrubber = Part2_Gen(new List<string>(alllines), bits, (s,c,zeros,ones) => (
                (ones >= zeros && s[c] == '0') || (ones < zeros && s[c] == '1')
            ));

            Console.Out.WriteLine($"Oxygen = {oxygen}, Scrubber = {scrubber}");
            Console.Out.WriteLine($"Result is {oxygen * scrubber}");
        }

        static int Part2_Gen(List<string> strings, int bits, Func<string,int,int,int,bool> test) {
            int c = 0;
            List<string> vals = strings;
            while (c < bits && vals.Count > 1) {
                Console.Out.WriteLine($"Starting with {vals.Count} numbers. Position is {c}");
                int ones = 0;
                int zeros = 0;
                
                foreach(var s in vals) {
                    if (s[c] == '1')
                        ones++;
                    else
                        zeros++;
                }
                Console.WriteLine($"{ones} ones and {zeros} zeros");
                List<string> ns = new List<string>();
                foreach(var s in vals) {
                    if (test(s,c,zeros,ones)) { 
                        Console.WriteLine($"Including {s} because at {c} there's a 1");
                        ns.Add(s);
                    }
                    else {
                        Console.WriteLine($"Excluding {s} because dominant is wrong at {c}");
                    }
                }
                vals = ns;
                c += 1;
            }
            Console.Out.WriteLine($"ns contains {vals.Count} values.");
            return stringToInt(vals[0]);
        }

        static public int stringToInt(string s) {
            int r = 0;
            for(int i = 0; i < s.Length; i++) {
                r = r * 2;
                if (s[i] == '1') r += 1;
            }
            Console.WriteLine($"{s} becomes {r}");
            return r;
        }
    }
}
