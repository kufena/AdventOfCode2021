using System;
using System.IO;

namespace day20
{
    class Program
    {
        static void Main(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);
            string illum = alllines[0];
            int rows = alllines.Length - 2;
            int cols = alllines[2].Length;
            int start = 2;

            int count = 0;
            for(int i = 2; i < alllines.Length; i++)
                for(int j = 0; j < alllines[i].Length; j++) {
                    count += alllines[i][j] == '#' ? 1 : 0;
                }
            Console.WriteLine($"COUNT == {count}");
            Console.WriteLine("");
            Console.WriteLine("");
            
            for(int k = 0; k < int.Parse(args[1]); k++) {
                int icols, irows;
                int pos = k % 2 == 0 ? 0 : 511;
                int fill = illum[pos] == '#' ? 1 : 0;
                var grid = CreateExtendedGrid(alllines, start, rows, cols, out irows, out icols, fill);

                string[] newrows = new string[irows-2];
                for(int i = 1; i < irows-1; i++) {
                    string s = "";
                    for(int j = 1; j < icols-1; j++) {
                        int ns = 
                        (grid[i-1][j-1] == 1 ? 256 : 0) +
                        (grid[i-1][j] == 1 ? 128 : 0) +
                        (grid[i-1][j+1] == 1 ? 64 : 0) +
                        (grid[i][j-1] == 1 ? 32 : 0) +
                        (grid[i][j] == 1 ? 16 : 0) +
                        (grid[i][j+1] == 1 ? 8 : 0) +
                        (grid[i+1][j-1] == 1 ? 4 : 0) +
                        (grid[i+1][j] == 1 ? 2 : 0) +
                        (grid[i+1][j+1] == 1 ? 1 : 0);
                        s += $"{illum[ns]}";
                    }
                    newrows[i-1] = s;
                }

                for(int i = 0; i < newrows.Length; i++)
                    Console.WriteLine(newrows[i]);
 
                start = 0;
                alllines = newrows;
                rows = newrows.Length;
                cols = newrows[0].Length;

                count = 0;
                for(int i = 0; i < newrows.Length-6; i++)
                    for(int j = 0; j < newrows[i].Length-6; j++) {
                        count += newrows[i+3][j+3] == '#' ? 1 : 0;
                    }
                Console.WriteLine($"COUNT == {count}");
                Console.WriteLine("");
                Console.WriteLine("");
                
            }
        
        }

        private static int[][] CreateExtendedGrid(string[] alllines, int start, int rows, int cols, out int irows, out int icols, int fill)
        {
            irows = rows + 6;
            icols = cols + 6;
            int[][] grid = new int[irows][];
            for (int i = 0; i < irows; i++)
            {
                grid[i] = new int[icols];
                for (int j = 0; j < icols; j++)
                    grid[i][j] = fill;
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i + 3][j + 3] = alllines[start+i][j] == '.' ? 0 : 1;
                }
            }
            return grid;
        }
    }
}
