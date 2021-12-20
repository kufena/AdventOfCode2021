using System;
using System.IO;
using System.Collections.Generic;

namespace day19 {

    public class ScannerGrid {
        public int[][][] grid { get; set; }
        public List<(int,int,int)> points { get; set; }

        public void buildGrid(List<(int,int,int)> _points) {
            points = new List<(int, int, int)>(_points); // take a copy, why not.
            int minx, miny, minz;
            int maxx, maxy, maxz;

            minmaxX(_points, (x => x.Item1), out minx, out maxx);
            minmaxX(_points, (x => x.Item2), out miny, out maxy);
            minmaxX(_points, (x => x.Item3), out minz, out maxz);
            
            int xsize = Extent(minx, maxx);
            int ysize = Extent(miny, maxy);
            int zsize = Extent(minz, maxz);

            Console.WriteLine($"xsize = {xsize} ysize = {ysize} zsize = {zsize}");
        }

        private int Extent(int a, int b) {
            return Math.Abs(a - b);
        }

        private void minmaxX(List<(int,int,int)> points, Func<(int,int,int),int> selector, out int min, out int max) {
            min = int.MaxValue;
            max = int.MinValue;
            foreach(var point in points) {
                int v = selector(point);
                if (v > max) max = v;
                if (v < min) min = v;
            }
        }
    }
}