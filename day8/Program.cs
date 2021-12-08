using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace day8
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        static void Part1(string[] args) {
            TextReader tr = new StreamReader(args[0]);
            string s;
            int count = 0;
            while ((s = tr.ReadLine()) != null) {
                // Just counting entries with 2,3,4 or 7 wire segments.
                var arr = s.Split(new char[] {' ','|'}, StringSplitOptions.RemoveEmptyEntries);
                for(int i = 10; i < 14; i++) {
                    int k = arr[i].Length;
                    if (k ==3 || k == 7 || k == 2 || k == 4)
                        count++;
                }
            }
            Console.Out.WriteLine($"Count is {count}");
        }

        static char[] zero = new char[] {'a','b','c','e','f','g'};
        static char[] one = new char[] {'c','f'};
        static char[] two = new char[] {'a','c','d','e','g'};
        static char[] three = new char[] {'a','c','d','f','g'};
        static char[] four = new char[] {'b','c','d','f'};
        static char[] five = new char[] {'a','b','d','f','g'};
        static char[] six = new char[] {'a','b','d','e','f','g'};
        static char[] seven = new char[] {'a','c','f'};
        static char[] eight = new char[] {'a','b','c','d','e','f','g'};
        static char[] nine = new char[] {'a','b','c','d','f','g'};
        
        static void Part2(string[] args) {
            // now we have to map the wire segments to find the actual digits in the code.
            // need to use the four digits we know, and clever stuff to get there.
            // 
            TextReader tr = new StreamReader(args[0]);
            string s;
            int count = 0;
            while ((s = tr.ReadLine()) != null) {
                var arr = s.Split(new char[] {' ','|'}, StringSplitOptions.RemoveEmptyEntries);
                    
                HashSet<char> ourseven = null;
                HashSet<char> ourone = null;
                HashSet<char> oureight = null;
                HashSet<char> ourfour = null;
                List<HashSet<char>> anyfiver = new List<HashSet<char>>();

                for(int i = 0; i < 10; i++) {
                    int k = arr[i].Length;
                    HashSet<char> us = new HashSet<char>();
                    for(int j = 0; j < k; j++)
                        us.Add(arr[i][j]);

                    if (k == 3) { // a seven
                        ourseven = us;
                    }
                    else if (k == 7) { // an eight
                        oureight = us;
                    }
                    else if (k == 2) { // a one
                        ourone = us;
                    }
                    else if (k == 4) { // a four
                        ourfour = us;
                    }
                    else if (k == 5)
                        anyfiver.Add(us);
                }

                Console.WriteLine($"We've done sometihng here");

                var ourA = Difference(ourseven, ourone);
                var ourCF = new HashSet<char>(ourone);
                var ourBD = Difference(ourfour, ourone);
                var ourEG = Difference(Difference(oureight, ourfour), ourA);
                HashSet<char> ourADG = null;
                HashSet<char> ourF = null;
                foreach(var S in anyfiver) {
                    ourADG = Difference(S,ourone);
                    if (ourADG.Count == 3)
                        break;
                }
                if (ourADG.Count != 3) {
                    throw new Exception("more than three chars in ADG");
                }
                var ourDG = Difference(ourADG, ourA);
                HashSet<char> ourC = null;
                foreach(var S in anyfiver) {
                    if (ourEG.IsSubsetOf(S)) {
                        // it's two, but means we can get cd?
                        ourC = Difference(Difference(Difference(S,ourEG),ourA),ourBD); // c.
                    }
                }
                var ourD = new HashSet<char>(ourBD);
                ourD.IntersectWith(ourDG);
                var ourG = new HashSet<char>(ourDG);
                ourG.IntersectWith(ourEG);
                var ourE = Difference(ourEG, ourG);
                var ourB = Difference(ourBD, ourD);
                ourF = Difference(ourone, ourC);

                var map = new Dictionary<char,char>();
                map.Add('a',ourA.First<char>());
                map.Add('b',ourB.First<char>());
                map.Add('c',ourC.First<char>());
                map.Add('d',ourD.First<char>());
                map.Add('e',ourE.First<char>());
                map.Add('f',ourF.First<char>());
                map.Add('g',ourG.First<char>());

                var inv = new Dictionary<char,char>();
                inv.Add(ourA.First<char>(),'a');
                inv.Add(ourB.First<char>(),'b');
                inv.Add(ourC.First<char>(),'c');
                inv.Add(ourD.First<char>(),'d');
                inv.Add(ourE.First<char>(),'e');
                inv.Add(ourF.First<char>(),'f');
                inv.Add(ourG.First<char>(),'g');
                
                Console.Out.WriteLine("we've got a map then.");

                int numb = 0;
                for(int i = 10; i < 14; i++) {
                    numb = numb * 10;
                    char[] ourchars = new char[arr[i].Length];
                    for(int j = 0; j < arr[i].Length; j++) {
                        ourchars[j] = inv[arr[i][j]];
                    }
                    Array.Sort(ourchars);
                    numb += FindChar(ourchars);
                }

                count += numb;
            }
            Console.Out.WriteLine($"Count is {count}");
        }

        static HashSet<char> Difference(HashSet<char> A, HashSet<char> B) {
            var res = new HashSet<char>();
            foreach(var c in A)
                if (!B.Contains(c))
                    res.Add(c);
            return res;
        }

        static int FindChar(char[] ourchars) {
            if (ArrayEquals(zero, ourchars)) return 0;
            if (ArrayEquals(one, ourchars)) return 1;
            if (ArrayEquals(two, ourchars)) return 2;
            if (ArrayEquals(three, ourchars)) return 3;
            if (ArrayEquals(four, ourchars)) return 4;
            if (ArrayEquals(five, ourchars)) return 5;
            if (ArrayEquals(six, ourchars)) return 6;
            if (ArrayEquals(seven, ourchars)) return 7;
            if (ArrayEquals(eight, ourchars)) return 8;
            if (ArrayEquals(nine, ourchars)) return 9;
            return -1;
        }

        static bool ArrayEquals(char[] A, char[] B) {
            if (A.Length != B.Length) return false;
            for(int i = 0; i < A.Length; i++) {
                if (A[i] != B[i]) return false;
            }
            return true;
        }
    }
}
