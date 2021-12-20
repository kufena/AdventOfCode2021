using System;
using System.IO;
using System.Collections.Generic;
using day19;

namespace day19 {

    public class Scanner {

        public List< ScannerGrid  > _grids { get; set; }
        public List<(int,int,int)> _points { get; set; }
        public int _id { get; }
        public Scanner(int id) {
            _points = new List<(int, int, int)>();
            _grids = new List<ScannerGrid>();
            _id = id;
        }

        public int[][] normplot() {

            int[][] norms = new int[_points.Count][];
            for(int i = 0; i < _points.Count; i++) {
                norms[i] = new int[_points.Count];
                for(int j = 0; j < _points.Count; j++) {
                    (var x1, var y1, var z1) = _points[i];
                    (var x2, var y2, var z2) = _points[j];
                    int n = (x1 - x2)*(x1-x2);
                    n += (y1-y2)*(y1-y2);
                    n += (z1-z2)*(z1-z2);
                    norms[i][j] = n;
                }
            }
            return norms;
        }

        public (int,int,int)[][] vectorplot() {
            (int,int,int)[][] vectors = new (int,int,int)[_points.Count][];
            for(int i = 0; i < _points.Count; i++) {
                vectors[i] = new (int,int,int)[_points.Count];
                for(int j = 0; j < _points.Count; j++) {
                    (var x1, var y1, var z1) = _points[i];
                    (var x2, var y2, var z2) = _points[j];
                    vectors[i][j] = (x1-x2,y1-y2,z1-z2);
                }
            }
            return vectors;
        }

        //
        // The twelve directions are:
        // x,y,z  x,z -y  x,-y,-z  x,-z,y
        // -x,y,-z  -x,-z,-y  -x,-y,z  -x,z,y
        // z,y,-x  z,-x,-y  z,-y,x  z,x,y
        // -z,x,y  -z,y,-x  -z,-x,-y  -z,-y,x
        // y,-x,z  y,z,x  y,x,-z  y,-z,-x
        // -y,-x,-z  -y,z,-x  -y,x,z  -y,-z,x
        public List<ScannerGrid> buildGrid() {

            List<Func<(int,int,int),(int,int,int)>> perms = new List<Func<(int, int, int), (int, int, int)>>();

            perms.Add(p => { (int x, int y, int z) = p; return (x,y,z); }); // x,y,z  x,z -y  x,-y,-z  x,-z,y
            perms.Add(p => { (int x, int y, int z) = p; return (x,z,-y); });
            perms.Add(p => { (int x, int y, int z) = p; return (x,-y,-z); });
            perms.Add(p => { (int x, int y, int z) = p; return (x,-z,y); });
            perms.Add(p => { (int x, int y, int z) = p; return (-x,y,-z); }); // -x,y,-z  -x,-z,-y  -x,-y,z  -x,z,y
            perms.Add(p => { (int x, int y, int z) = p; return (-x,-z,-y); });
            perms.Add(p => { (int x, int y, int z) = p; return (-x,-y,z); });
            perms.Add(p => { (int x, int y, int z) = p; return (-x,z,y); });
            perms.Add(p => { (int x, int y, int z) = p; return (z,y,-x); }); // z,y,-x  z,-x,-y  z,-y,x  z,x,y
            perms.Add(p => { (int x, int y, int z) = p; return (z,-x,-y); });
            perms.Add(p => { (int x, int y, int z) = p; return (z,-y,x); });
            perms.Add(p => { (int x, int y, int z) = p; return (z,x,y); });
            perms.Add(p => { (int x, int y, int z) = p; return (-z,x,y); }); // -z,x,y  -z,y,-x  -z,-x,-y  -z,-y,x
            perms.Add(p => { (int x, int y, int z) = p; return (-z,y,-x); });
            perms.Add(p => { (int x, int y, int z) = p; return (-z,-x,-y); });
            perms.Add(p => { (int x, int y, int z) = p; return (-z,-y,x); });
            perms.Add(p => { (int x, int y, int z) = p; return (y,-x,z); }); // y,-x,z  y,z,x  y,x,-z  y,-z,-x
            perms.Add(p => { (int x, int y, int z) = p; return (y,z,x); });
            perms.Add(p => { (int x, int y, int z) = p; return (y,x,-z); });
            perms.Add(p => { (int x, int y, int z) = p; return (y,-z,-x); });
            perms.Add(p => { (int x, int y, int z) = p; return (-y,-x,-z); }); // -y,-x,-z  -y,z,-x  -y,x,z  -y,-z,x
            perms.Add(p => { (int x, int y, int z) = p; return (-y,z,-x); });
            perms.Add(p => { (int x, int y, int z) = p; return (-y,x,z); });
            perms.Add(p => { (int x, int y, int z) = p; return (-y,-z,x); });

            List<(int,int,int)> directions = new List<(int, int, int)>();
            directions.Add((1,1,1));
            
            foreach(var perm in perms) {
                foreach(var direction in directions) {
                    var sg = new ScannerGrid();
                    List<(int,int,int)> myPoints = new List<(int, int, int)>(_points);
                    foreach(var p in _points)
                        myPoints.Add(perm((p.Item1 * direction.Item1, p.Item2 * direction.Item2, p.Item3 * direction.Item3)));
                    sg.buildGrid(myPoints);
                    _grids.Add(sg);
                }
            }

            return _grids;           
        }


        public void Duplicates() {
            for(int i = 0; i < _grids.Count; i++) {
                for(int j = i+1; j < _grids.Count; j++) {
                    var alist = new List<(int,int,int)>(_grids[i].points);
                    var blist = _grids[j].points;
                    bool same = true;
                    foreach(var p in blist) {
                        if (!alist.Contains(p)) {
                            same = false; 
                            break;
                        }
                        alist.Remove(p);
                    }
                    if (same && alist.Count == 0) {
                        Console.WriteLine($"Scanner {_id} :: Numero {i} is the same as {j}");
                    }
                }
            }
        }
    }

}