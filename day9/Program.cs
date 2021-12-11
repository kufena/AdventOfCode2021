using System;
using System.Collections.Generic;
using System.IO;

namespace day9
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        static void Part1(string[] args) {
            var alllines = File.ReadAllLines(args[0]);
            int[][] grid = new int[alllines.Length][];

            int maxrow = alllines.Length;
            int maxcol = alllines[0].Length;

            for(int i = 0; i < maxrow; i++) {
                grid[i] = new int[maxcol];
                for(int j = 0; j < maxcol; j++) {
                    grid[i][j] = int.Parse("" + alllines[i][j]); // Ugh!
                }
            }

            int risk = 0;
            for(int row = 0; row < maxrow; row++) {
                for(int col = 0; col < maxcol; col++) {
                    risk += PointRisk(grid, row, col);
                }
            }

            Console.Out.WriteLine($"Total risk is {risk}");
        }

        
        static void Part2(string[] args) {
            var alllines = File.ReadAllLines(args[0]);
            int[][] grid = new int[alllines.Length][];

            int maxrow = alllines.Length;
            int maxcol = alllines[0].Length;

            for(int i = 0; i < maxrow; i++) {
                grid[i] = new int[maxcol];
                for(int j = 0; j < maxcol; j++) {
                    grid[i][j] = int.Parse("" + alllines[i][j]); // Ugh!
                }
            }

            int[] maxthree = new int[] {int.MinValue, int.MinValue, int.MinValue};
            for(int row = 0; row < maxrow; row++) {
                for(int col = 0; col < maxcol; col++) {
                    if (PointRisk(grid, row, col) > 0) // it's a low point
                    {
                        int basinSize = FindBasin(grid,row,col);

                        if (basinSize > maxthree[0]) {
                            maxthree[2] = maxthree[1];
                            maxthree[1] = maxthree[0];
                            maxthree[0] = basinSize;
                        } else if (basinSize > maxthree[1]) {
                                maxthree[2] = maxthree[1];
                                maxthree[1] = basinSize;
                        } else if (basinSize > maxthree[2]) {
                                maxthree[2] = basinSize;
                        }
                    }
                }
            }

            Console.Out.WriteLine($"Top three were {maxthree[0]}, {maxthree[1]} and {maxthree[2]}");
            Console.Out.WriteLine($"Total basin sum is {maxthree[0] * maxthree[1] * maxthree[2]}");
        }

        private static int FindBasin(int[][] grid, int row, int col)
        {
            var result = BasinPoints(grid, row, col, 1, -1);
            result.UnionWith(BasinPoints(grid, row, col, 1, 1));
            result.UnionWith(BasinPoints(grid, row, col, -1, 1));
            result.UnionWith(BasinPoints(grid, row, col, -1, -1));

            Console.Out.WriteLine($"Point {row} {col} has a basin size of {result.Count}");
            return result.Count;
        }

        static HashSet<(int,int)> BasinPoints(int[][] grid, int row, int col, int rowd, int cold) {
            var result = new HashSet<(int,int)>();
            if (row < 0 || col < 0 || row >= grid.Length || col >= grid[row].Length)
                return result;
            if (grid[row][col] == 9)
                return result;
            result.Add((row,col));
            var dirone = BasinPoints(grid, row + rowd, col, rowd, cold);
            var dirtwo = BasinPoints(grid, row, col + cold, rowd, cold);
            //var dirthr = BasinPoints(grid, row + rowd, col + cold, rowd, cold);

            result.UnionWith(dirone);
            result.UnionWith(dirtwo);

            return result;
        }

        static int PointRisk(int[][] grid, int row, int col) {
            int rowup = int.MaxValue;
            int rowdown = int.MaxValue;
            int colleft = int.MaxValue;
            int colright = int.MaxValue;

            if (row - 1 >= 0) rowup = grid[row-1][col];
            if (row + 1 < grid.Length) rowdown = grid[row+1][col];
            if (col - 1 >= 0) colleft = grid[row][col-1];
            if (col + 1 < grid[0].Length) colright = grid[row][col+1];

            int point = grid[row][col];
            if (point < rowup && point < rowdown && point < colleft && point < colright) {
                Console.Out.WriteLine($"Point {row} {col} is a low.");
                return 1 + point;
            }
            else   
                return 0;
        }
    }
}
