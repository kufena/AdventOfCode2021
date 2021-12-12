using System;
using System.IO;
using System.Collections.Generic;

namespace day12
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string,int> namesToIndices;
            Dictionary<int,string> indicesToNames;
            var graph = ReadGraph(args, out namesToIndices, out indicesToNames);

            //var paths = FindPaths(namesToIndices["start"], namesToIndices["end"], graph, namesToIndices, indicesToNames, new List<int>());
            var paths = FindPaths2(namesToIndices["start"], 
                                    namesToIndices["end"], graph, namesToIndices, indicesToNames, 
                                    new List<int>() {}, false, 0);
            foreach(var p in paths) {
                string path = "";
                foreach(var s in p) {
                    path = $"{indicesToNames[s]}-{path}";
                }
                Console.Out.WriteLine(path);
            }
            Console.Out.WriteLine($"Found {paths.Count} paths");
        }

        private static int[][] ReadGraph(string[] args, out Dictionary<string,int> namesToIndices, out Dictionary<int, string> indicesToNames)
        {
            var alllines = File.ReadAllLines(args[0]);
            namesToIndices = new Dictionary<string, int>();
            indicesToNames = new Dictionary<int, string>();
            int count = 0;
            for(int i = 0; i < alllines.Length; i++) {
                var items = alllines[i].Split('-',StringSplitOptions.RemoveEmptyEntries);
                var source = items[0];
                var dest = items[1];
                if (!namesToIndices.ContainsKey(source)) {
                    namesToIndices.Add(source, count);
                    indicesToNames.Add(count, source);
                    count++;
                }
                if (!namesToIndices.ContainsKey(dest)) {
                    namesToIndices.Add(dest, count);
                    indicesToNames.Add(count, dest);
                    count++;
                }
            }    

            int nameCount = namesToIndices.Keys.Count;
            int[][] graph = new int[nameCount][];
            for(int i = 0; i < nameCount; i++) {
                graph[i] = new int[nameCount];
                for(int j = 0; j < nameCount; j++)
                    graph[i][j] = -1;
            }

            for(int i = 0; i < alllines.Length; i++) {
                var items = alllines[i].Split('-',StringSplitOptions.RemoveEmptyEntries);
                var source = items[0];
                var dest = items[1];
                graph[namesToIndices[source]][namesToIndices[dest]] = 1;
                graph[namesToIndices[dest]][namesToIndices[source]] = 1;
            }
            return graph;
        }

        // Find paths from source to dest.
        static List<List<int>> FindPaths(int source, int dest, 
                              int[][] graph, Dictionary<string,int> namesToIndices, Dictionary<int, string> indicesToNames,
                              List<int> visited) {

            if (visited.Contains(source)) // look out for loops I guess.
                return new List<List<int>>();

            if (source == dest) { // we're here already!
                return new List<List<int>>() { new List<int>() {dest} };
            }

            // ok find routes out of here and go for it.
            if (LowerCase(indicesToNames[source]))
                visited.Add(source);

            var result = new List<List<int>>();
            for(int j = 0; j < graph[source].Length; j++) {
                if (source == j) continue;
                if (graph[source][j] == -1)
                    continue;
                
                var paths = FindPaths(j, dest, graph, namesToIndices, indicesToNames, new List<int>(visited));
                foreach(var p in paths) {
                    p.Add(source);
                    result.Add(p);
                }
            }
            return result;
        }

        // Find paths from source to dest.
        static List<List<int>> FindPaths2(int source, int dest, 
                              int[][] graph, Dictionary<string,int> namesToIndices, Dictionary<int, string> indicesToNames,
                              List<int> visited, bool multipleset, int multiple) {

            if (multipleset && multiple == source)
                return new List<List<int>>();

            if (multipleset && multiple != source && visited.Contains(source))
                return new List<List<int>>();

            if (visited.Contains(source) && source == namesToIndices["start"])
                return new List<List<int>>();
            
            if (source == dest) { // we're here already! should always be end.
                return new List<List<int>>() { new List<int>() {dest} };
            }

            var result = new List<List<int>>();
            for(int j = 0; j < graph[source].Length; j++) {
                if (source == j) continue;
                if (graph[source][j] == -1)
                    continue;
            
                if (!multipleset && LowerCase(indicesToNames[source]) && visited.Contains(source)) {
                    var newvisited = new List<int>(visited);
                    if (LowerCase(indicesToNames[source]))
                        newvisited.Add(source);
                    var paths = FindPaths2(j, dest, graph, namesToIndices, indicesToNames, newvisited, true, source);
                    foreach(var p in paths) {
                        p.Add(source);
                        result.Add(p);
                    }    
                } else {
                    var newvisited = new List<int>(visited);
                    if (LowerCase(indicesToNames[source]))
                        newvisited.Add(source);
                    var paths = FindPaths2(j, dest, graph, namesToIndices, indicesToNames, newvisited, multipleset, multiple);
                    foreach(var p in paths) {
                        p.Add(source);
                        result.Add(p);
                    }
                }
            }
            return result;
        }

        private static bool LowerCase(string v)
        {
            return char.IsLower(v[0]);
        }
    }
}
