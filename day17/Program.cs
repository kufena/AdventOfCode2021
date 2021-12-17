using System;
using System.IO;

namespace day17
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1(args);
        }

        private static void Part1(string[] args)
        {
            string input = File.ReadAllLines(args[0])[0];
            var nums = input.Substring(13).Split(new char[] {' ','.',',','='}, StringSplitOptions.RemoveEmptyEntries);
            int botx = int.Parse(nums[1]);
            int boty = int.Parse(nums[4]);
            int topx = int.Parse(nums[2]);
            int topy = int.Parse(nums[5]);

            Console.WriteLine($"From {topx},{topy} to {botx},{boty}");

            // perform steps.

            int countInBox = 0;
            for(int x = -botx; x <= 2*botx; x++) {
                for(int y = 8*boty; y < (8*Math.Abs(boty)); y++) {

                    int xpos = 0;
                    int ypos = 0;

                    int xvel = x;
                    int yvel = y;

                    int heighesty = int.MinValue;

                    while(true) {

                        if ((xpos >= botx && xpos <= topx) &&
                            (ypos <= topy && ypos >= boty)) { // we've reached box
                            Console.WriteLine($"In the box ffor {x} {y}");
                            countInBox += 1;
                            break;
                        }

                        if (xpos > topx || ypos < boty) { // we've gone past
                            heighesty = int.MinValue;
                            break;
                        }

                        xpos += xvel;
                        ypos += yvel;

                        if (ypos > heighesty)
                            heighesty = ypos;

                        StepVelocityChange(xvel, yvel, out xvel, out yvel);
                    }

                    if (heighesty >= 0) 
                        Console.WriteLine($"For xvel={x} yvel={y} max height was {heighesty}");
                }
            }
            Console.WriteLine($"In box = {countInBox}");
                
        }

        private static void StepVelocityChange(int x, int y, out int xx, out int yy) {
            // Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
            xx = x - 1;
            if (xx < 0) xx = 0;

            // Due to gravity, the probe's y velocity decreases by 1.
            yy = y - 1;
        }
    }
}
