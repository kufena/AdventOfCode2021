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
            // Read the file, get the initial polymer string.
            var alllines = File.ReadAllLines(args[0]);
            var polymerStart = alllines[0];

            // Read and parse the rules.
            Dictionary<string, string> rules = new Dictionary<string, string>();
            for (int i = 2; i < alllines.Length; i++)
            {
                var p = alllines[i].Split(new char[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                rules.Add(p[0], p[1]);
            }

            // We'll pass the number of steps as an argument.
            int steps = int.Parse(args[1]);

            // Split the original polymer string in to pairs of letters, so we can
            // apply the rules.  If the same string appears more than once, we just
            // count the strings we have.  Order doesn't seem to matter in the end.
            Dictionary<string,long> pairCounts = new Dictionary<string, long>();
            for(int i = 0; i < polymerStart.Length-1; i++) {
                string pref = polymerStart.Substring(i,2);
                if (pairCounts.ContainsKey(pref))
                    pairCounts[pref] += 1;
                else
                    pairCounts.Add(pref,1);
            }

            // We'll keep a count of each letter as we go.
            var letterCounts = CountLettersInString(polymerStart);

            // DO THE STEPS.
            for(int step = 0; step < steps; step++) {

                // We'll keep a dictionary of pairs, which will become the input
                // to the next step.
                var newPairCounts = new Dictionary<string,long>();
                string pref = "";
                char lastrulechar = ' ';
                long lastrulecount = 0;

                // So, let's go through each pair in the polymer.
                foreach((string p, long c) in pairCounts) {

                    // we'll label them pref and lastrulecount, which we might need later.
                    pref = p;
                    lastrulecount = c;

                    // if the pair triggers a rule...
                    if (rules.ContainsKey(pref))
                    {
                        // ... if we have, say, 8 lots of NN, and the rule insert a C between
                        // them, then we create the new pairs NC and CN and add them to the
                        // new dictionary of pair counts, increasing the count by 8 for each one.
                        string npref = $"{pref[0]}{rules[pref]}";
                        string ppref = $"{rules[pref]}{pref[1]}";
                        lastrulechar = rules[pref][0];
                        UpdatePairs(newPairCounts, c, npref);
                        UpdatePairs(newPairCounts, c, ppref);

                        // with our example, we'll also want to increase the letter counts for
                        // C by 8 as well.  It's the only letter we've added, so that's all we add.
                        UpdateCounts(letterCounts, newPairCounts, lastrulechar, c, npref);                        
                    }

                    // if no rule is triggered, then we just put it in the new list, as is.
                    else {
                        UpdatePairs(newPairCounts, c, pref);
                    }
                }

                // Once we have done all pairs of letters in the old counts, we update pairCounts
                // to the dictionary we've created in this step, ready for the next step.
                pairCounts = newPairCounts;

            }

            // Find the smallest and do some output with the results we need for the input.
            char smallest, largest;
            long smallestCount, largestCount;
            FindLargestSmallest(letterCounts, out smallest, out smallestCount, out largest, out largestCount);

            Console.Out.WriteLine($"Commonest is {largest} with count {largestCount}");
            Console.Out.WriteLine($"Least common is {smallest} with {smallestCount}");
            Console.Out.WriteLine($"Sum is {largestCount - smallestCount}");

        }

        //
        // Sure there's a way to do this in one line, but not sure how.
        private static void UpdatePairs(Dictionary<string, long> newPairCounts, long c, string npref)
        {
            if (newPairCounts.ContainsKey(npref))
                newPairCounts[npref] += c;
            else newPairCounts[npref] = c;
        }

        //
        // I'm sure you can do this in one line anyway, but not sure how.
        private static void UpdateCounts(Dictionary<char, long> letterCounts, Dictionary<string, long> newPairCounts, char lastrulechar, long c, string npref)
        {
            if (letterCounts.ContainsKey(lastrulechar))
                letterCounts[lastrulechar] += c;
            else
                letterCounts[lastrulechar] = c;
        }


        //
        // Naive version of the algorithm.
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

        //
        // Given a dictionary of counts of chars, find the smallest counted char and the
        // largest counted char.
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

        //
        // I'm sure there's a better way of doing this, but hey.
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
