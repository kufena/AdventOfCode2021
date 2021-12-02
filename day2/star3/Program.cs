using System;
using System.IO;

namespace star3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's navigate!");
            int horizontal = 0;
            int vertical = 0;

            TextReader tr = new StreamReader("input.txt");
            string ln;
            // Parse and interpret the commands as per.
            while ((ln = tr.ReadLine()) != null) {
                if (ln.StartsWith("up")) {
                    vertical -= int.Parse(ln.Substring(3));
                }
                if (ln.StartsWith("down")) {
                    vertical += int.Parse(ln.Substring(5));
                }
                if (ln.StartsWith("forward")) {
                    horizontal += int.Parse(ln.Substring(8));
                }
            }

            Console.Out.WriteLine($"Horizontal = {horizontal}, Vertical = {vertical}");
            Console.Out.WriteLine($"Sum is {horizontal * vertical}");
        }
    }
}
