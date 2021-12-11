using System;
using System.Collections.Generic;
using System.IO;

namespace day11
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1(args);
        }

        static void Part1(string[] args) {
            var alllines = File.ReadAllLines(args[0]);
            int[][] octopodii = new int[alllines.Length][];
            for(int i = 0; i < alllines.Length; i++) {
                octopodii[i] = new int[alllines[i].Length];
                for(int j = 0; j < alllines[i].Length; j++) {
                    octopodii[i][j] = ToInt(alllines[i][j]);
                }
            }

            int maxrows = alllines.Length;
            int maxcols = octopodii[0].Length;

            // display initial grid.
            Console.Out.WriteLine("Start!");
            for(int row = 0; row < maxrows; row++) {
                for(int col = 0; col < maxcols; col++) {
                    Console.Out.Write($"{octopodii[row][col]}");
                }
                Console.Out.WriteLine("");
            }

            // perform steps.
            int steps = int.Parse(args[1]);

            int flashes = 0;
            int firstfull = -1;

            for(int i = 0; i < steps; i++) {

                // do initial step increase.
                for(int row = 0; row < maxrows; row++) {
                    for(int col = 0; col < maxcols; col++) {
                        octopodii[row][col] += 1;
                        //Flash(row,col,octopodii,maxrows,maxcols);
                    }
                }

                // do changes increase
                int changes = 0;
                List<(int,int)> alreadyflashed = new List<(int, int)>(); // arg! annoying.
                while (true) {
                    changes = 0;
                    List<(int,int)> toflash = new List<(int, int)>();
                    for(int row = 0; row < maxrows; row++) {
                        for(int col = 0; col < maxcols; col++) {
                            if (octopodii[row][col] > 9 && !alreadyflashed.Contains((row,col))) // only just got to flash
                            {
                                toflash.Add((row,col));
                            }
                        }
                    }

                    //Console.Out.Write("Flashing");
                    //foreach((var r, var c) in toflash) Console.Out.Write($" (r,c)-");
                    //Console.Out.WriteLine("");

                    if (toflash.Count > 0) {
                        foreach((var r, var c) in toflash) {
                            Flash(r,c,octopodii,maxrows,maxcols);
                            changes += 1;
                            alreadyflashed.Add((r,c));
                        }
                    }
                    flashes += toflash.Count;

                    if (changes == 0)
                        break;
                }

                // threshold.
                int tot = 0;
                for(int row = 0; row < maxrows; row++) {
                    for(int col = 0; col < maxcols; col++) {
                        if (octopodii[row][col] > 9)
                            octopodii[row][col] = 0;
                        else
                            tot += octopodii[row][col];
                    }
                }

                if (tot == 0 && firstfull < 0)
                    firstfull = i;
                
                // display grid after step.
                Console.Out.WriteLine("");
                Console.Out.WriteLine("Step " + i);
                for(int row = 0; row < maxrows; row++) {
                    for(int col = 0; col < maxcols; col++) {
                        Console.Out.Write($"{octopodii[row][col]}");
                    }
                    Console.Out.WriteLine("");
                }

                //Console.In.ReadLine();
            }

            Console.Out.WriteLine($"Flashes = {flashes}");
            Console.Out.WriteLine($"First full flash is at step {firstfull+1}");
        }

        private static void Flash(int row, int col, int[][] octopodii, int maxrows, int maxcols)
        {
            if (row - 1 >= 0) {
                octopodii[row-1][col] += 1;
                if (col - 1 >= 0)
                    octopodii[row-1][col-1] += 1;
                if (col + 1 < maxcols)
                    octopodii[row-1][col+1] += 1;
            }
            if (row + 1 < maxrows) {
                octopodii[row+1][col] += 1;
                if (col - 1 >= 0)
                    octopodii[row+1][col-1] += 1;
                if (col + 1 < maxcols)
                    octopodii[row+1][col+1] += 1;
            }
        
            if (col - 1 >= 0)
                octopodii[row][col-1] += 1;
            if (col + 1 < maxcols)
                octopodii[row][col+1] += 1;

            octopodii[row][col] += 1; // we do this to inhibit further changes.
        }

        private static int ToInt(char v)
        {
            // probably a better way to do this, but hey.
            int x = (v == '0') ? 0 :
                    (v == '1') ? 1 :
                    (v == '2') ? 2 :
                    (v == '3') ? 3 :
                    (v == '4') ? 4 :
                    (v == '5') ? 5 :
                    (v == '6') ? 6 :
                    (v == '7') ? 7 :
                    (v == '8') ? 8 :
                    (v == '9') ? 9 : -1;
            return x;
        }
    }
}
