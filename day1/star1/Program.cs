using System;
using System.IO;

namespace star1
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader tr = new StreamReader("input.txt");
            long last = long.Parse(tr.ReadLine());
            string nx;
            int x = 0;
            while ((nx = tr.ReadLine()) != null)
            {
                long n = long.Parse(nx);
                if (n > last)
                    x++;
                last = n;
            }
            Console.Out.WriteLine(x);
        }

    }
}
