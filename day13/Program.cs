using System;
using System.IO;
using System.Collections.Generic;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1(args);
        }

        private static void Part1(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);

            List<(int,int)> pairs = new List<(int,int)>();
            int maxx = 0;
            int maxy = 0;

            int foldspos = 0;
            for(int i = 0; i < alllines.Length; i++) {
                if (alllines[i].Trim() == "") {
                    foldspos = i + 1;
                    break;
                }

                var pair = alllines[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(pair[0]);
                int y = int.Parse(pair[1]);

                pairs.Add((y,x));
                if (x > maxx) maxx = x;
                if (y > maxy) maxy = y;
            }

            maxx += 1;
            maxy += 1;

            int[][] grid = new int[maxy][];
            for(int i = 0; i < maxy; i++) {
                grid[i] = new int[maxx];
                for(int j = 0; j < maxx; j++)
                    grid[i][j] = 1;
            }

            foreach((var y, var x) in pairs) {
                grid[y][x] = 0;
            }

            // First fold only.
            for (int k =foldspos; k < alllines.Length; k++) {
                var strs = alllines[k].Split(new char[] {' ','='}, StringSplitOptions.RemoveEmptyEntries);
                int newmaxx;
                int newmaxy;

                grid = FoldGrid(grid, strs[2], int.Parse(strs[3]), maxx, maxy, out newmaxx, out newmaxy);
                maxx = newmaxx;
                maxy = newmaxy;
            }

            int totdots = 0;
            for (int i = 0; i < maxy; i++) {
                for (int j = 0; j < maxx; j++) {
                    totdots += (grid[i][j] == 0 ? 1 : 0);
                    Console.Write(grid[i][j] == 0 ? "#" : ".");
                }
                Console.WriteLine("");
            }
            Console.Out.WriteLine("");
            Console.Out.WriteLine($"Total dots is {totdots}");
        }

        private static int[][] FoldGrid(int[][] grid, string axis, int line, int maxx, int maxy, out int newmaxx, out int newmaxy)
        {
            newmaxx = -1;
            newmaxy = -1; 

            // line here is the actual index in the grid, ie incl 0.
            // so new grid is line+1 new sized, but fold up to line-1 merges?
            int[][] newgrid;
            if (axis == "y") {
                int lastline = (2 * line) + 1;
                newgrid = new int[line][];
                for(int i = 0; i < (line); i++) {
                    newgrid[i] = grid[i]; // all the old lines
                }
                int c = 0;
                for(int j = lastline-1; j > line; j--) {
                    for(int k = 0; k < maxx; k++) {
                        if (newgrid[c][k] == 0 || grid[j][k] == 0)
                            newgrid[c][k] = 0;
                        else
                            newgrid[c][k] = 1;
                    }
                    c += 1;
                }
                newmaxx = maxx;
                newmaxy = line;
                return newgrid;
            }

            if (axis == "x") {
                int lastline = (2 * line);
                newgrid = new int[maxy][];
                for(int i = 0; i < maxy; i++) {
                    newgrid[i] = new int[line];
                    for(int k = 0; k < line; k++)
                        newgrid[i][k] = grid[i][k]; // half the old lines
                }
                for(int j = 0; j < maxy; j++) {
                    int c = 0;
                    for(int k = lastline; k > line; k--) {
                        if (newgrid[j][c] == 0 || grid[j][k] == 0)
                            newgrid[j][c] = 0;
                        else
                            newgrid[j][c] = 1;
                        c += 1;
                    }

                }
                newmaxy = maxy;
                newmaxx = line;
                return newgrid;
            }

            return grid;
        }
    }
}
