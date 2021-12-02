using System;
using System.IO;

namespace star4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Let's navigate!");
            int horizontal = 0;
            int vertical = 0;
            int aim = 0;

            TextReader tr = new StreamReader("input.txt");
            string ln;
            // Parse and interpret the commands as per.
            while ((ln = tr.ReadLine()) != null) {
                if (ln.StartsWith("up")) {
                    int n = int.Parse(ln.Substring(3));
                    //vertical -= n;
                    aim -= n;
                    Console.Out.WriteLine($"up {n} to reach {vertical} with aim {aim}");
                }
                if (ln.StartsWith("down")) {
                    int n = int.Parse(ln.Substring(5));
                    //vertical += n;
                    aim += n;
                    Console.Out.WriteLine($"down {n} to reach {vertical} with aim {aim}");
                }
                if (ln.StartsWith("forward")) {
                    int n = int.Parse(ln.Substring(8));
                    horizontal += n;
                    vertical += (aim * n);
                    Console.Out.WriteLine($"forward {n} to reach {horizontal} with aim {aim * n} taking v to {vertical}");

                }
            }

            Console.Out.WriteLine($"Horizontal = {horizontal}, Vertical = {vertical}");
            Console.Out.WriteLine($"Sum is {horizontal * vertical}");
        }
    }
}
