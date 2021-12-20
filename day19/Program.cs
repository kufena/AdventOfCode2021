using System;
using System.IO;
using System.Collections.Generic;

namespace day19
{
    class Program
    {
        static void Main(string[] args)
        {
            var scanners = ParseFile(args[0]);
            Console.Out.WriteLine($"Found {scanners.Count} scanners.");

            Dictionary<int,List<ScannerGrid>> scannerDict = new Dictionary<int, List<ScannerGrid>>();

            foreach(var scan in scanners) {
                if (scan._id == 0) {
                    ScannerGrid sg = new ScannerGrid() { points = scan._points };
                    scannerDict.Add(scan._id, new List<ScannerGrid>() { sg });
                } else {
                    var allsgs = scan.buildGrid();
                    scannerDict.Add(scan._id, allsgs);
                }
            }

            var successes = new Dictionary<int, Dictionary<int, List<(ScannerGrid, ScannerGrid)>>>();
            var successtr = new Dictionary<int, Dictionary<int, List<(int,int,int)>>>();

            for(int i = 0; i < scanners.Count; i++) {
                for(int j = i+1; j < scanners.Count; j++) {
                    Console.WriteLine($"Comparing scanner {scanners[i]._id} and {scanners[j]._id}");
                    var listi = scannerDict[i];
                    var listj = scannerDict[j];
                    foreach(var sgi in listi) {
                        foreach(var sgj in listj) {
                            int xtrans, ytrans, ztrans;
                            if (PointMatcherHelper.MatchPoints(sgi.points, sgj.points, out xtrans, out ytrans, out ztrans)) {
                                if (!successes.ContainsKey(i))
                                    successes.Add(i, new Dictionary<int, List<(ScannerGrid, ScannerGrid)>>());
                                var sub = successes[i];
                                if (!sub.ContainsKey(j))
                                    sub.Add(j, new List<(ScannerGrid, ScannerGrid)>());
                                sub[j].Add((sgi,sgj));
                                if (!successtr.ContainsKey(i)) successtr.Add(i,new Dictionary<int,List<(int,int,int)>>());
                                if (!successtr[i].ContainsKey(j)) successtr[i].Add(j, new List<(int,int,int)>());
                                successtr[i][j].Add((xtrans,ytrans,ztrans));
                            }
                        }
                    }

                    // can we reduce a bit?
                    if (successes.ContainsKey(i) && successes[i].ContainsKey(j) && successes[i][j].Count > 0) {
                        var l = successes[i][j];
                        var jlist = scannerDict[j];
                        scannerDict[j] = new List<ScannerGrid>();
                        foreach(var pair in l)
                            scannerDict[j].Add(pair.Item2);
                    }
                }
            }

            Console.WriteLine("wat wat wat?");

            List<(int,int,int)> allpoints = new List<(int, int, int)>();
            var positions = new Dictionary<int, HashSet<(int, int, int)>>();
            positions.Add(0,new HashSet<(int, int, int)>() { (0,0,0) });
            foreach((var key, var dict) in successes) {
                if (positions.ContainsKey(key)) {
                    foreach((var ox, var oy, var oz) in positions[key]) {
                        foreach((var tar, var tarlist) in dict) {
                            foreach((var tx, var ty, var tz) in successtr[key][tar]) {
                                //int xpos = ox - tx;
                                //int ypos = oy - ty;
                                //int zpos = oz - tz;
                                int xpos = tx + ox;
                                int ypos = ty + oy;
                                int zpos = tz + oz;
                                if (!positions.ContainsKey(tar)) positions.Add(tar, new HashSet<(int,int,int)>());
                                positions[tar].Add((xpos,ypos,zpos));
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Well, we won't get here!");
        }
        
        private static void OverlapViaNorms(List<Scanner> scanners)
        {
            for (int i = 1; i < scanners.Count; i++)
            {
                var vex1 = scanners[i].vectorplot();
                var nor1 = scanners[i].normplot();
                for (int j = i + 1; j < scanners.Count; j++)
                {
                    var vex2 = scanners[j].vectorplot();
                    var nor2 = scanners[j].normplot();
                    int overlaps = FindVectorOverlaps(scanners, vex1, vex2);
                    int noverlaps = FindNormOverlaps(scanners, nor1, nor2);
                    Console.Out.WriteLine($"Found {overlaps} overlap potentials btw {i} and {j}");
                    Console.Out.WriteLine($"Found {noverlaps} overlap potentials btw {i} and {j}");
                }
            }
        }

        private static int FindVectorOverlaps(List<Scanner> scanners, (int, int, int)[][] vex1, (int, int, int)[][] vex2)
        {
            // a norm is an nxn matrix but we're only interested in bottom triangle?
            int overlaps = 0;
            for (int k = 1; k < scanners.Count; k++)
            {
                for (int l = 0; l < k; l++)
                {
                    if (OverlapVectors(vex2, vex1[k][l]))
                        overlaps += 1;
                }
            }

            return overlaps;
        }

        private static int FindNormOverlaps(List<Scanner> scanners, int[][] vex1, int[][] vex2)
        {
            // a norm is an nxn matrix but we're only interested in bottom triangle?
            int overlaps = 0;
            for (int k = 1; k < scanners.Count; k++)
            {
                for (int l = 0; l < k; l++)
                {
                    if (Overlap(vex2, vex1[k][l]))
                        overlaps += 1;
                }
            }

            return overlaps;
        }

        private static bool OverlapVectors((int,int,int)[][] norm2, (int,int,int) p)
        {
            (var x, var y, var z) = p;
            for(int i = 1; i < norm2.Length; i++) {
                for(int j = 0; j < i; j++) {
                    (var xc, var yc, var zc) = norm2[i][j];
                    if (x==xc && y==yc && z==zc) 
                        return true;
                }
            }
            return false;
        }

        private static bool Overlap(int[][] norm2, int v)
        {
            for(int i = 1; i < norm2.Length; i++) {
                for(int j = 0; j < i; j++) {
                    if (norm2[i][j] == v) return true;
                }
            }
            return false;
        }

        static List<Scanner> ParseFile(string file) {

            List<Scanner> result = new List<Scanner>();
            var alllines = File.ReadAllLines(file);
            int counter = 0;
            while(true) {

                var suff = alllines[counter].Substring(12).Split(' ');
                int id = int.Parse(suff[0]);
                Scanner s = new Scanner(id);

                counter += 1;
                while (counter < alllines.Length && alllines[counter].Trim() != "") {
                    var xyz = alllines[counter].Split(',', StringSplitOptions.RemoveEmptyEntries);

                    counter += 1;
                    int x = int.Parse(xyz[0]);
                    int y = int.Parse(xyz[1]);
                    int z = int.Parse(xyz[2]);
                    s._points.Add((x,y,z));
                }
                result.Add(s);
                counter += 1;
                if (counter >= alllines.Length)
                    break;
            }

            return result;
        }
    }
}
