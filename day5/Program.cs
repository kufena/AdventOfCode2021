using System;
using System.Collections.Generic;
using System.IO;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Part2(args);
        }

        public static void Part2(string[] args) {
            var alllines = File.ReadAllLines(args[0]);
            List<((int,int),(int,int))> pertinent = new List<((int,int),(int,int))>();

            int maxx = 0;
            int maxy = 0;

            for(int i = 0; i < alllines.Length; i++) {
                (var x1, var y1, var x2, var y2) = ParseLine(alllines[i]);
                pertinent.Add(((x1,y1),(x2,y2)));
                if (x1 > maxx) maxx = x1;
                if (x2 > maxx) maxx = x2;
                if (y1 > maxy) maxy = y1;
                if (y2 > maxy) maxy = y2;
            }
            maxx += 1;
            maxy += 1;

            Console.Out.WriteLine($"We have {pertinent.Count} lines, in a grid {maxx} by {maxy}");

            int[][] grid = new int[maxy][];
            for(int i = 0; i < maxy; i++) {
                grid[i] = new int[maxx];
                for(int j = 0; j < maxx; j++)
                    grid[i][j] = 0;
            }

            foreach(var point in pertinent) {
                ((var x1, var y1),(var x2, var y2)) = point;
                if (x1 == x2) {
                    int low = y1 < y2 ? y1 : y2;
                    int high = y1 < y2 ? y2 : y1;
                    for (int col = low; col <= high; col++) {
                        grid[col][x1] += 1;
                    }
                }
                else if (y1 == y2) {
                    int low = x1 < x2 ? x1 : x2;
                    int high = x1 < x2 ? x2 : x1;
                    for (int row = low; row <= high; row++) {
                        grid[y1][row] += 1;
                    }
                }
                else {
                    int stepx = x1 < x2 ? 1 : -1;
                    int stepy = y1 < y2 ? 1 : -1;
                    int sx = x1;
                    int sy = y1;
                    bool tobreak = false;
                    while(true) {
                        grid[sy][sx] += 1;
                        sy += stepy;
                        sx += stepx;
                        if (tobreak)
                            break;
                        if (sy == y2 || sx == x2)
                            tobreak = true; // break next time round.
                    }
                }
            }
            
            int finalCount = 0;
            for(int i = 0; i < maxy; i++) {
                for(int j = 0; j < maxx; j++) {
                    //if (grid[i][j] == 0) Console.Out.Write("."); else Console.Out.Write($"{grid[i][j]}");
                    if (grid[i][j] > 1) finalCount++;

                }
                //Console.Out.WriteLine("");
            }

            Console.Out.WriteLine($"Final Count is {finalCount}");

        }

        public static void Part1(string[] args) {
            var alllines = File.ReadAllLines(args[0]);
            List<((int,int),(int,int))> pertinent = new List<((int,int),(int,int))>();

            int maxx = 0;
            int maxy = 0;

            for(int i = 0; i < alllines.Length; i++) {
                (var x1, var y1, var x2, var y2) = ParseLine(alllines[i]);
                if (x1 == x2 || y1 == y2) {
                    pertinent.Add(((x1,y1),(x2,y2)));
                    if (x1 > maxx) maxx = x1;
                    if (x2 > maxx) maxx = x2;
                    if (y1 > maxy) maxy = y1;
                    if (y2 > maxy) maxy = y2;
                }
            }
            maxx += 1;
            maxy += 1;

            Console.Out.WriteLine($"We have {pertinent.Count} lines, in a grid {maxx} by {maxy}");

            int[][] grid = new int[maxy][];
            for(int i = 0; i < maxy; i++) {
                grid[i] = new int[maxx];
                for(int j = 0; j < maxx; j++)
                    grid[i][j] = 0;
            }

            foreach(var point in pertinent) {
                ((var x1, var y1),(var x2, var y2)) = point;
                if (x1 == x2) {
                    int low = y1 < y2 ? y1 : y2;
                    int high = y1 < y2 ? y2 : y1;
                    for (int col = low; col <= high; col++) {
                        grid[col][x1] += 1;
                    }
                }
                else {
                    int low = x1 < x2 ? x1 : x2;
                    int high = x1 < x2 ? x2 : x1;
                    for (int row = low; row <= high; row++) {
                        grid[y1][row] += 1;
                    }
                }
            }
            
            int finalCount = 0;
            for(int i = 0; i < maxy; i++) {
                for(int j = 0; j < maxx; j++) {
                    if (grid[i][j] == 0) Console.Out.Write("."); else Console.Out.Write($"{grid[i][j]}");
                    if (grid[i][j] > 1) finalCount++;

                }
                Console.Out.WriteLine("");
            }

            Console.Out.WriteLine($"Final Count is {finalCount}");

        }

        public static (int,int,int,int) ParseLine(string s) {
            var arr = s.Split(new char[] {' ','-','>',','}, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length != 4) throw new Exception($"string failure {s}");
            return (int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]), int.Parse(arr[3]));
        }
    }
}
