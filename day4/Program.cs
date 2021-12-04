using System;
using System.Collections.Generic;
using System.IO;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
//            Part2(new string[] {"test.txt"}); //args);
            Part2(args);
        }

        static void Part1(string[] args) {
            var alllines = File.ReadAllLines(args[0]);

            int[] numbers = ParseNumbers(alllines[0],',');
            
            int boardCounts = (alllines.Length - 1) / 6;

            int next = 2;

            int count = int.MaxValue;
            long rcVal = 0;
            long ttVal = 0;

            for(int j = 0; j < boardCounts; j++) {
                int[][] board = new int[5][];
                Dictionary<int,(int,int)> map = new Dictionary<int,(int,int)>(); // map values to (row,col)
                bool[][] ticks = new bool[5][];
                for(int row = 0; row < 5; row++) {
                    board[row] = ParseNumbers(alllines[next+row],' ');
                    ticks[row] = new bool[] {false,false,false,false,false};
                    for(int col = 0; col < 5; col++)
                        map.Add(board[row][col],(row,col)); // k is row, l is col
                }

                for(int k = 0; k < numbers.Length; k++) {
                    if (map.ContainsKey(numbers[k])) {
                        (var row, var col) = map[numbers[k]];
                        ticks[row][col] = true;
                        bool wCol = checkWinCol(ticks, (row,col));
                        bool wRow = checkWinRow(ticks, (row,col));
                        if ((wCol || wRow) && (k < count)) {
                            count = k;
                            rcVal = 0;
                            ttVal = 0;
                            for(int row_i = 0; row_i < 5; row_i++) {
                                for(int col_j = 0; col_j < 5; col_j++) {
                                    if (!ticks[row_i][col_j])
                                        ttVal += board[row_i][col_j];
                                }
                            }
                            rcVal = numbers[k];
                            //for(int i = 0; i < 5; i++) {
                            //    if (wCol) rcVal += board[i][col];
                            //    else rcVal += board[row][i];
                            //}
                            break;
                        }
                    }
                }
                next += 6;
            }

            Console.Out.WriteLine($"We have a solution = {count} {rcVal} {ttVal}");
            Console.Out.WriteLine($"Sum is {ttVal * rcVal}");

        }

        static void Part2(string[] args) {
            var alllines = File.ReadAllLines(args[0]);

            int[] numbers = ParseNumbers(alllines[0],',');
            
            int boardCounts = (alllines.Length - 1) / 6;

            int next = 2;

            int count = int.MinValue;
            long rcVal = 0;
            long ttVal = 0;

            for(int j = 0; j < boardCounts; j++) {
                int[][] board = new int[5][];
                Dictionary<int,(int,int)> map = new Dictionary<int,(int,int)>(); // map values to (row,col)
                bool[][] ticks = new bool[5][];
                for(int row = 0; row < 5; row++) {
                    board[row] = ParseNumbers(alllines[next+row],' ');
                    ticks[row] = new bool[] {false,false,false,false,false};
                    for(int col = 0; col < 5; col++)
                        map.Add(board[row][col],(row,col)); // k is row, l is col
                }

                for(int k = 0; k < numbers.Length; k++) {
                    if (map.ContainsKey(numbers[k])) {
                        (var row, var col) = map[numbers[k]];
                        ticks[row][col] = true;
                        bool wCol = checkWinCol(ticks, (row,col));
                        bool wRow = checkWinRow(ticks, (row,col));
                        if ((wCol || wRow)) { // we have a win, so that's ok
                            if (k > count) {
                                count = k;
                                rcVal = 0;
                                ttVal = 0;
                                for(int row_i = 0; row_i < 5; row_i++) {
                                    for(int col_j = 0; col_j < 5; col_j++) {
                                        if (!ticks[row_i][col_j])
                                            ttVal += board[row_i][col_j];
                                    }
                                }
                                rcVal = numbers[k];
                                //for(int i = 0; i < 5; i++) {
                                //    if (wCol) rcVal += board[i][col];
                                //    else rcVal += board[row][i];
                                //}
                            }

                            break; // we'll break here anyway.
                        }
                        
                    }
                }
                next += 6;
            }

            Console.Out.WriteLine($"We have a solution = {count} {rcVal} {ttVal}");
            Console.Out.WriteLine($"Sum is {ttVal * rcVal}");

        }

        private static int[] ParseNumbers(string v, char c)
        {
            Console.Out.WriteLine($"parsing {v}");
            var arr = v.Split(c, StringSplitOptions.RemoveEmptyEntries);
            int[] res = new int[arr.Length];
            for(int i = 0; i < arr.Length; i++)
                res[i] = int.Parse(arr[i]);
            return res;
        }

        private static bool checkWinCol(bool[][] ticks, (int,int) coord) {
            (var row, var col) = coord;
            bool r = true;
            for(int i = 0; i < 5; i++) 
                r = r && ticks[i][col];
            return r;
        }

        private static bool checkWinRow(bool[][] map, (int,int) coord) {
            (var row, var col) = coord;
            bool r = true;
            for(int i = 0; i < 5; i++) 
                r = r && map[row][i];
            return r;
        }
    }
}
