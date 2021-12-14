using System;
using System.IO;
using System.Collections.Generic;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        private static void Part2(string[] args) {
            var alllines = File.ReadAllLines(args[0]);
            var polymerStart = alllines[0];
            Dictionary<string, string> rules = new Dictionary<string, string>();

            for (int i = 2; i < alllines.Length; i++)
            {
                var p = alllines[i].Split(new char[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(p[0], p[1]);
            }

            int steps = int.Parse(args[1]);

            Dictionary<string,long> pairCounts = new Dictionary<string, long>();
            for(int i = 0; i < polymerStart.Length-1; i++) {
                string pref = polymerStart.Substring(i,2);
                if (pairCounts.ContainsKey(pref))
                    pairCounts[pref] += 1;
                else
                    pairCounts.Add(pref,1);
            }

            var letterCounts = CountLettersInString(polymerStart);

            for(int step = 0; step < steps; step++) {

                // Do pair counts.
                var newPairCounts = new Dictionary<string,long>();
                string pref = "";
                bool correct = false;
                char lastrulechar = ' ';
                long lastrulecount = 0;
                foreach((string p, long c) in pairCounts) {
                    pref = p;
                    lastrulecount = c;
                    if (rules.ContainsKey(pref))
                    {
                        string npref = $"{pref[0]}{rules[pref]}";
                        string ppref = $"{rules[pref]}{pref[1]}";
                        lastrulechar = rules[pref][0];
                        correct = true;
                        UpdatePairs(letterCounts, newPairCounts, lastrulechar, c, npref);
                        UpdateCounts(letterCounts, newPairCounts, lastrulechar, c, npref);
                        UpdatePairs(letterCounts, newPairCounts, lastrulechar, c, ppref);                        
                    }
                    else {
                        correct = false;
                        if (newPairCounts.ContainsKey(pref))
                            newPairCounts[pref] += c;
                        else newPairCounts[pref] = c;
                    }
                }
                /*
                if (correct) {
                    string npref = $"{lastrulechar}{pref[1]}";

                    if (newPairCounts.ContainsKey(npref))
                        newPairCounts[npref] += lastrulecount;
                    else newPairCounts[npref] = lastrulecount;
                    if (letterCounts.ContainsKey(lastrulechar))
                        letterCounts[lastrulechar] += lastrulecount;
                    else
                        letterCounts[lastrulechar] = lastrulecount;
                        
                }
                */
                pairCounts = newPairCounts;

            }

            char smallest, largest;
            long smallestCount, largestCount;
            FindLargestSmallest(letterCounts, out smallest, out smallestCount, out largest, out largestCount);

            Console.Out.WriteLine($"Commonest is {largest} with count {largestCount}");
            Console.Out.WriteLine($"Least common is {smallest} with {smallestCount}");
            Console.Out.WriteLine($"Sum is {largestCount - smallestCount}");

        }

        private static void UpdatePairs(Dictionary<char, long> letterCounts, Dictionary<string, long> newPairCounts, char lastrulechar, long c, string npref)
        {
            if (newPairCounts.ContainsKey(npref))
                newPairCounts[npref] += c;
            else newPairCounts[npref] = c;
        }

        private static void UpdateCounts(Dictionary<char, long> letterCounts, Dictionary<string, long> newPairCounts, char lastrulechar, long c, string npref)
        {
            if (letterCounts.ContainsKey(lastrulechar))
                letterCounts[lastrulechar] += c;
            else
                letterCounts[lastrulechar] = c;
        }


        private static void Part1(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);
            var polymerStart = alllines[0];
            Dictionary<string, string> rules = new Dictionary<string, string>();

            for (int i = 2; i < alllines.Length; i++)
            {
                var p = alllines[i].Split(new char[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(p[0], p[1]);
            }

            int steps = int.Parse(args[1]);
            string polymer = polymerStart;

            for (int step = 0; step < steps; step++)
            {

                string newpoly = "";
                for (int pos = 0; pos < polymer.Length - 1; pos++)
                {
                    string prefix = polymer.Substring(pos, 2);
                    if (rules.ContainsKey(prefix))
                    {
                        newpoly += $"{polymer[pos]}{rules[prefix]}";
                    }
                    else
                    {
                        newpoly += $"{polymer[pos]}";
                    }
                }
                newpoly += polymer[polymer.Length - 1];
                polymer = newpoly;
                Console.Out.WriteLine($"Step {step}: {polymer.Length}");
            }

            Dictionary<char, long> counts = CountLettersInString(polymer);

            char smallest, largest;
            long smallestCount, largestCount;
            FindLargestSmallest(counts, out smallest, out smallestCount, out largest, out largestCount);

            Console.Out.WriteLine($"Polymer has length {polymer.Length}");
            Console.Out.WriteLine($"Commonest is {largest} with count {largestCount}");
            Console.Out.WriteLine($"Least common is {smallest} with {smallestCount}");
            Console.Out.WriteLine($"Sum is {largestCount - smallestCount}");

        }

        private static void FindLargestSmallest(Dictionary<char, long> counts, out char smallest, out long smallestCount, 
                                                                               out char largest, out long largestCount)
        {
            smallest = ' ';
            smallestCount = long.MaxValue;
            largest = ' ';
            largestCount = 0;
            foreach ((var k, var v) in counts)
            {
                if (v < smallestCount)
                {
                    smallestCount = v;
                    smallest = k;
                }
                if (v > largestCount)
                {
                    largestCount = v;
                    largest = k;
                }
            }
        }

        private static Dictionary<char, long> CountLettersInString(string polymer)
        {
            Dictionary<char, long> counts = new Dictionary<char, long>();
            for (int i = 0; i < polymer.Length; i++)
            {
                if (counts.ContainsKey(polymer[i]))
                    counts[polymer[i]] += 1;
                else
                    counts.Add(polymer[i], 1);
            }

            return counts;
        }
    }
}
