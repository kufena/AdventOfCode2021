using System;
using System.IO;

namespace star1
{
    class Program
    {
        static void Main(string[] args)
        {
            TextReader tr = new StreamReader("input.txt");
            // Get first number
            long last = long.Parse(tr.ReadLine());
            string nx;
            int x = 0;
            while ((nx = tr.ReadLine()) != null)
            {
                //get next number, compare with last one
                long n = long.Parse(nx);
                if (n > last)
                    x++;
                //current number becomes the last
                last = n;
            }
            Console.Out.WriteLine(x);
        }

    }
}
