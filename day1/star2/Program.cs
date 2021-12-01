﻿using System;
using System.Collections.Generic;
using System.IO;

namespace star2
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader tr = new StreamReader("input.txt");
            List<long> l = new List<long>();
            string s;
            while ((s = tr.ReadLine()) != null) {
                long k = long.Parse(s);
                l.Add(k);
            }
            var arr = l.ToArray();
            long[] narr = new long[arr.Length - 2];
            for(int i = 0; i < arr.Length-2; i++) {
                narr[i] = arr[i] + arr[i+1] + arr[i+2];
            }
            int x = 0;
            for(int i = 1; i < narr.Length; i++){
                if (narr[i] > narr[i-1])
                    x++;
            }
            Console.Out.WriteLine(x);
        }
    }
}
