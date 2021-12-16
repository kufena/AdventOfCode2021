using System;
using System.Collections.Generic;
using System.IO;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        private static void Part1(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);
            int maxrow = alllines.Length;
            int maxcol = alllines[0].Length;

            int[][] grid = new int[maxrow][];
            for(int i = 0; i < maxrow; i++) {
                grid[i] = new int[maxcol];
                for(int j = 0; j < maxcol; j++) {
                    grid[i][j] = int.Parse(alllines[i].Substring(j,1)); // just a digit.
                }
            }

            Console.Out.WriteLine($"Got a grid {maxrow} rows and {maxcol} cols");
            int[][] scores = new int[maxrow][];
            (int,int)[][] directions = new (int,int)[maxrow][];
            for(int row = maxrow-1; row >= 0; row--) {
                scores[row] = new int[maxcol];
                directions[row] = new (int,int)[maxcol];
                for(int col = maxcol-1; col >= 0; col--) {

                    scores[row][col] = grid[row][col];
                    directions[row][col] = (-1,-1);
                    int down = int.MaxValue;
                    int back = int.MaxValue;
                    if (row + 1 < maxrow) {
                        down = scores[row+1][col];
                    }
                    if (col + 1 < maxcol) {
                        back = scores[row][col+1];
                    }

                    if (row == maxrow-1 && col == maxcol-1) // we didn't go anywhere.
                        continue;
                    else {
                        if (back < down) {
                            scores[row][col] += back;
                            directions[row][col] = (row,col+1);
                        }
                        else {
                            scores[row][col] += down;
                            directions[row][col] = (row+1,col);
                        }
                    }
                }
            }

            scores[0][0] -= grid[0][0];
            Console.Out.WriteLine($"We've done something!");
            Console.Out.WriteLine($"Score is {scores[0][0]}");

        }

        static int highestScore = 400;
        static long[][] gridscores;

        private static void Part2(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);
            int maxrow = alllines.Length;
            int maxcol = alllines[0].Length;

            int origmaxrow = maxrow;
            int origmaxcol = maxcol;

            int[][] protogrid = new int[maxrow][];
            for(int i = 0; i < maxrow; i++) {
                protogrid[i] = new int[maxcol];
                for(int j = 0; j < maxcol; j++) {
                    int v = int.Parse(alllines[i].Substring(j,1));
                    protogrid[i][j] = v;
                }
            }

            int[][] grid = new int[maxrow*5][];
            for(int i = 0; i < maxrow*5; i++)
                grid[i] = new int[maxcol*5];

            for(int i = 0; i < maxrow; i++) {
                for(int j = 0; j < maxcol; j++) {
                    for(int k = 0;  k < 5; k++) {
                        int v = protogrid[i][j] + k;
                        if (v > 9) v = v - 9;
                        grid[i+(k*origmaxrow)][j] = v;                    }
                }
            }
            maxrow = maxrow * 5;
            for(int i = 0; i < maxrow; i++) {
                for(int j = 0; j < maxcol; j++) {
                    for(int k = 0; k < 5; k++) {
                        int v = grid[i][j] + k;
                        if (v > 9) v = v - 9;
                        grid[i][j+(k*origmaxcol)] = v;                    }
                }
            }
            maxcol = maxcol * 5;
            gridscores = new long[origmaxrow][];
            for(int k = 0; k < origmaxrow; k++) {
                gridscores[k] = new long[origmaxcol];
                for(int l = 0; l < origmaxcol; l++) {
                    gridscores[k][l] = -1;
                }
            }

            for(int i = 0; i < maxrow; i++) {
                for(int j = 0; j < maxcol; j++) {
                    if (grid[i][j] > 9)
                        Console.Out.WriteLine($"We've gone over 9 at {i} {j} = {grid[i][j]}");
                    Console.Out.Write($"{grid[i][j]} ");
                }
                Console.Out.WriteLine("");
            }

            Console.Out.WriteLine($"Got a grid {maxrow} rows and {maxcol} cols");
            int[][] scores = new int[maxrow][];
            (int,int)[][] directions = new (int,int)[maxrow][];
            for(int row = maxrow-1; row >= 0; row--) {
                scores[row] = new int[maxcol];
                directions[row] = new (int,int)[maxcol];
                for(int col = maxcol-1; col >= 0; col--) {

                    scores[row][col] = grid[row][col];
                    directions[row][col] = (-1,-1);
                    int down = int.MaxValue;
                    int back = int.MaxValue;
                    if (row + 1 < maxrow) {
                        down = scores[row+1][col];
                    }
                    if (col + 1 < maxcol) {
                        back = scores[row][col+1];
                    }

                    if (row == maxrow-1 && col == maxcol-1) // we didn't go anywhere.
                        continue;
                    else {
                        if (back < down) {
                            scores[row][col] += back;
                            directions[row][col] = (row,col+1);
                        }
                        else {
                            scores[row][col] += down;
                            directions[row][col] = (row+1,col);
                        }
                    }
                }
            }

            scores[0][0] -= grid[0][0];
            Console.Out.WriteLine($"We've done something!");
            Console.Out.WriteLine($"Score is {scores[0][0]}");

//            FindLowest(0,0,grid, maxrow - 1, maxcol - 1, new List<(int, int)>() { (0,0) }, 0);
//            for(int i = origmaxrow-1; i >= 0; i--) {
//                for(int j = origmaxcol-1; j >= 0; j--) {
//                    long vr = FindLowest(i,j,protogrid, origmaxrow - 1, origmaxcol - 1, new List<(int, int)>() { (i,j) }, protogrid[i][j]);
//                    gridscores[i][j] = vr;
//                }
//            }

//            Console.Out.WriteLine($"We've done something!");
//            Console.Out.WriteLine($"Score is {highestScore}");
//            Console.Out.WriteLine($"Return is {vr}");
//            Console.Out.WriteLine($"Grid scores value is {gridscores[0][0]}");
        }

        private static long FindLowest(int row, int col, int[][] grid, int endrow, int endcol, List<(int,int)> seen, int score)
        {
            Stack<(int,int)> nextPoints = new Stack<(int, int)>();
            
            if (row == endrow && col == endcol) {
                if (score < highestScore)
                    //highestScore = score;
                    return grid[endrow][endcol];
            }
            
            if (gridscores[row][col] > -1)
                return gridscores[row][col];

            if (!seen.Contains((row+1,col)) && row+1 <= endrow)
                nextPoints.Push((row+1,col));
            if (!seen.Contains((row,col+1)) && col+1 <= endcol)
                nextPoints.Push((row,col+1));
            if (!seen.Contains((row-1,col)) && row-1 > 0)
                nextPoints.Push((row-1,col));
            //if (!seen.Contains((row,col-1)) && col-1 > 0)
            //    nextPoints.Push((row,col-1));

            long min = long.MaxValue;
            foreach((var r, var c) in nextPoints) {
                int newscore = score + grid[r][c];
                var newseen = new List<(int,int)>(seen);
                newseen.Add((r,c));
                if (newscore > highestScore) 
                    continue;
                long t = FindLowest(r, c, grid, endrow, endcol, newseen, newscore);
                if (t > 0 && t < min)
                    min = t;
            }            
            if (min < long.MaxValue) {
                gridscores[row][col] = min + grid[row][col];
                return min + grid[row][col];
            }
            return -1;
            
        }
    }
}
