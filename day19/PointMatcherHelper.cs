using System;
using System.IO;
using System.Collections.Generic;

namespace day19
{
    class PointMatcherHelper {

        public static bool Match(Scanner s1, Scanner s2) {

            var s1grids = s1._grids;
            var s2grids = s2._grids;

            foreach(var grid1 in s1grids) {
                foreach(var grid2 in s2grids) {
                    int xtrans,ytrans,ztrans;
                    if (MatchPoints(grid1.points, grid2.points, out xtrans, out ytrans, out ztrans))
                        return true;
                }
            }
            return false;
        }

        public static bool MatchPoints(List<(int,int,int)> psetA, List<(int,int,int)> psetB, out int xtrans, out int ytrans, out int ztrans) {
            xtrans = 0;
            ytrans = 0;
            ztrans = 0;
            foreach((var pax, var pay, var paz) in psetA) { // choose a random point in A
            
                foreach((var pbx, var pby, var pbz) in psetB) { // match to one in B
                
                    xtrans = pax - pbx;
                    ytrans = pay - pby;
                    ztrans = paz - pbz;

                    int matchCount = 0;
                    foreach((var pxx, var pyy, var pzz) in psetB) {
                        if (psetA.Contains((pxx + xtrans, pyy + ytrans, pzz + ztrans))) {
                            matchCount++;
                        }
                    }
                    if (matchCount >= 12) {
                        Console.WriteLine($"We've found {matchCount} matches between two lots of points!  Hurrah!");
                        Console.WriteLine($"Translation {xtrans} {ytrans} {ztrans}!  Hurrah!");
                        return true;
                    }
                
                }
            }
            return false;
        }
    }
}