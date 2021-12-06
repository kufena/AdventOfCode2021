using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        static void Part1(string[] args) {
            var strings = File.ReadAllLines(args[0])[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<int> start = new List<int>();
            foreach(var s in strings)
                start.Add(int.Parse(s));

            int days = int.Parse(args[1]);

            int count = 0;
            var list = new List<int>(start);
            while (count < days) {

                int births = list.Count(x => x == 0);
                if (births > 0) {
                    list.RemoveAll(x => x == 0);
                
                    List<int> nl = new List<int>();
                    foreach(var l in list) nl.Add(l - 1);
                    for(int i = 0; i < births; i++) {
                        nl.Add(6);
                        nl.Add(8);
                    }

                    list = nl;
                    count += 1;
                }
                else {
                    int next = list.Min<int>();
                    List<int> nl = new List<int>();
                    foreach(var x in list)
                        nl.Add(x - next);
                    list = nl;
                    count += next;
                }

                Console.Out.WriteLine($"did {count} days");
                Console.Out.WriteLine($"Left with {list.Count} rays");

            }

        }
        public static void Part2(string[] args) {
            var strings = File.ReadAllLines(args[0])[0].Split(',', StringSplitOptions.RemoveEmptyEntries);

            long[] lookAhead = new long[9] {0,0,0,0,0,0,0,0,0};

            foreach(var s in strings) {
                lookAhead[int.Parse(s)] += 1;
            }

            int days = int.Parse(args[1]);

            for(int i = 0; i < days; i++) {

                long zeroCount = lookAhead[0];
                for(int j = 1; j < 9; j++) {
                    lookAhead[j-1] = lookAhead[j];
                }
                lookAhead[6] += zeroCount;
                lookAhead[8] = zeroCount;
            }

            long total = 0;
            for(int i = 0; i < 9; i++) total += lookAhead[i];

            Console.WriteLine($"Total is {total}");

        }
    }
}
