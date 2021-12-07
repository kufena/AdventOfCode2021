using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        static void Part1(string[] args) {
            string[] ss = File.ReadAllLines(args[0])[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<int> nums = new List<int>();
            for(int i = 0; i < ss.Length; i++) nums.Add(int.Parse(ss[i]));
            int max = nums.Max();
            int min = nums.Min();

            Dictionary<int,int> counts = new Dictionary<int, int>();
            foreach(var n in nums) {
                if (counts.ContainsKey(n))
                    counts[n] += 1;
                else
                    counts.Add(n,1);
            }
            
            int pos = -1;
            int fuel = int.MaxValue;

            foreach(var key in counts.Keys) {
                int f = 0;
                foreach(var pot in counts.Keys) {
                    f += (counts[pot]) * Math.Abs(key - pot);
                }
                if (f < fuel) {
                    fuel = f;
                    pos = key;
                }
            }

            Console.WriteLine($"Position {pos} with fuel {fuel}");
        }

        static Dictionary<int,long> memo = new Dictionary<int, long>();
        static long fib(int n) {
            if (memo.ContainsKey(n)) return memo[n];
            long res = 0;
            if (n == 0) res = 0;
            else if (n == 1) res = 1;
            else res = ((long)n) + fib(n-1);
            memo.Add(n,res);
            return res;
        }

        static void Part2(string[] args) {
            string[] ss = File.ReadAllLines(args[0])[0].Split(',', StringSplitOptions.RemoveEmptyEntries);
            List<int> nums = new List<int>();
            for(int i = 0; i < ss.Length; i++) nums.Add(int.Parse(ss[i]));
            int max = nums.Max();
            int min = nums.Min();

            Dictionary<int,int> counts = new Dictionary<int, int>();
            foreach(var n in nums) {
                if (counts.ContainsKey(n))
                    counts[n] += 1;
                else
                    counts.Add(n,1);
            }
            
            int pos = -1;
            long fuel = long.MaxValue;

            for(int key = min; key <= max; key++) {
                long f = 0;
                foreach(var pot in counts.Keys) {
                    f += ((long)counts[pot]) * fib(Math.Abs(key - pot));
                }
                if (f < fuel) {
                    fuel = f;
                    pos = key;
                }
            }

            Console.WriteLine($"Position {pos} with fuel {fuel}");
        }
    }
}
