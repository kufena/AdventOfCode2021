using System;
using System.IO;
using System.Collections.Generic;

namespace day18
{
    class Program
    {
        static void Main(string[] args)
        {
            //ExplodeTesting();
            //SplitTesting();
            //Part1(args);
            Part2(args);
        }

        private static void SplitTesting() {
            int v;
            var tree1 = ParseNumber("[10,3]", 0, out v, 1);
            TestSplit(tree1);
        }

        private static void TestSplit(Tree tree1) {
            tree1.print();
            Console.Out.WriteLine("");
            Split(tree1, false);
            tree1.print();
            Console.Out.WriteLine("");        }

        private static void ExplodeTesting()
        {
            int v;
            var tree1 = ParseNumber("[[[[[9,8],1],2],3],4]", 0, out v, 1); // becomes [[[[0,9],2],3],4]
            var tree2 = ParseNumber("[7,[6,[5,[4,[3,2]]]]]", 0, out v, 1); // [7,[6,[5,[4,[3,2]]]]] becomes [7,[6,[5,[7,0]]]] 
            var tree3 = ParseNumber("[[6,[5,[4,[3,2]]]],1]", 0, out v, 1); // [[6,[5,[7,0]]],3]
            var tree4 = ParseNumber("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", 0, out v, 1); //  becomes [[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]
            var tree5 = ParseNumber("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", 0, out v, 1); //  becomes [[3,[2,[8,0]]],[9,[5,[7,0]]]]
            TestExplode(tree1);
            TestExplode(tree2);
            TestExplode(tree3);
            TestExplode(tree4);
            TestExplode(tree5);
        }

        private static void TestExplode(Tree tree1)
        {
            tree1.print();
            Console.Out.WriteLine("");
            Explode(tree1, false);
            tree1.print();
            Console.Out.WriteLine("");
        }

        private static void Part2(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);
            Tree[] l = new Tree[alllines.Length];
            int newpos = 0;
            for(int i = 0; i < alllines.Length; i++ ) {
                first_leaf = null;
                last_leaf = null;
                var t = ParseNumber(alllines[i], 0, out newpos, 1);
                l[i] = t; //l.Add(t);
                Console.WriteLine(alllines[i]);
                t.print();
                Console.WriteLine("");
            }

            Console.WriteLine($"Found {l.Length} trees.");

            long largestMag = 0;
            for(int i = 0; i < l.Length; i++) {
                for(int j = 0; j < l.Length; j++) {
                    if (i != j) {
                        long mag;
                        AddTrees(Copy(l[i]),Copy(l[j]),out mag);
                        if (mag > largestMag)
                            largestMag = mag;
                    }
                }
            }

            Console.Out.WriteLine($"Largest magnitude is {largestMag}");
        }

        private static void Part1(string[] args)
        {
            var alllines = File.ReadAllLines(args[0]);
            List<Tree> l = new List<Tree>();
            int newpos = 0;
            for(int i = 0; i < alllines.Length; i++ ) {
                first_leaf = null;
                last_leaf = null;
                var t = ParseNumber(alllines[i], 0, out newpos, 1);
                l.Add(t);
                Console.WriteLine(alllines[i]);
                t.print();
                Console.WriteLine("");
            }

            Console.WriteLine($"Found {l.Count} trees.");

            var tree1 = l[0];
            for(int i = 1; i < l.Count; i++)
            {
                var tree2 = l[i];
                long mag;
                Node newtree = AddTrees(tree1, tree2, out mag);

                tree1 = newtree;
            }

            Console.WriteLine("Added them all up to get::");
            l[0].print();
            Console.WriteLine("");
        }

        private static Node AddTrees(Tree tree1, Tree tree2, out long magnitude)
        {
            Console.WriteLine($"ADDING TWO TREES:::");
            tree1.print();
            Console.WriteLine("");
            tree2.print();
            Console.WriteLine("");
            Console.WriteLine("");

            // need to link the leafs, and add one to depths = not this easy!
            var newtree = new Node() { left = tree1, right = tree2, depth = 1 };
            tree1.parent = newtree;
            tree2.parent = newtree;
            IncDepth(tree1);
            IncDepth(tree2);
            var rl = FindRightLeaf(tree1);
            var ll = FindLeftLeaf(tree2);

            rl.right_leaf = ll;
            ll.left_leaf = rl;

            // need to reduce here.

            Reduce(newtree);
            magnitude = Magnitude(newtree);
            Console.WriteLine("ANSWER == ");
            newtree.print();
            Console.WriteLine("");
            Console.WriteLine($"MAGNITUDE == {magnitude}");
            Console.WriteLine("");
            return newtree;
        }

        private static void IncDepth(Tree t) {
            switch (t) {
                case Leaf l:
                    l.depth += 1;
                    break;
                case Node n:
                    {
                        n.depth += 1;
                        IncDepth(n.left);
                        IncDepth(n.right);
                    }
                    break;
            }
        }
        private static void Reduce(Tree t) {
            bool b = true;
            while (b) {
                b = Explode(t, false);
                b = b || Split(t, false); 
            }
        }

        private static Leaf FindLeftLeaf(Tree t) {
            switch(t) {
                case Node n: {
                    return FindLeftLeaf(n.left);
                }
                case Leaf l:
                    return l;

                default:
                    return null;
            }
        }
        private static Leaf FindRightLeaf(Tree t) {
            switch(t) {
                case Node n: {
                    return FindRightLeaf(n.right);
                }
                case Leaf l:
                    return l;

                default:
                    return null;
            }
        }

        private static long Magnitude(Tree t) {
            switch(t) {
                case (Leaf n):
                    return (long)(n.value);

                case (Node n): {
                    long m1 = Magnitude(n.left);
                    long m2 = Magnitude(n.right);
                    return (m1*3) + (m2*2);
                }

                default:
                    return 0;
            }
        }
        
        private static bool Explode(Tree t, bool b) {
            switch(t) {
                case (Leaf n):
                    return false;

                case (Node n):
                {
                    Leaf left;
                    Leaf right;
                    if (ExpCandidate(n, out left, out right)) {
                        Node par = (Node) n.parent;
                        
                        // Add left value to first value to the left,
                        if (left.left_leaf != null) left.left_leaf.value += left.value;
                        // Add right value to the first value to the right.
                        if (right.right_leaf != null) right.right_leaf.value += right.value;

                        // build new Leaf.
                        Leaf newleaf = new Leaf() { value = 0, parent = par, depth = n.depth };
                        newleaf.left_leaf = left.left_leaf;
                        if (left.left_leaf != null) left.left_leaf.right_leaf = newleaf;
                        newleaf.right_leaf = right.right_leaf;
                        if (right.right_leaf != null) right.right_leaf.left_leaf = newleaf;

                        // replace exploding pair with 0.
                        if (par.left == n) {
                            par.left = newleaf;
                        }
                        else {
                            par.right = newleaf;
                        }

                        return true;
                    }
                    else {
                        bool bb = Explode(n.left, b);
                        if (!bb) bb = Explode(n.right, b);
                        return bb;
                    }
                }

                default:
                    return false;
            }
        }

        private static bool ExpCandidate(Tree t, out Leaf left, out Leaf right) {
            left = null;
            right = null;

            switch (t) {
                case (Node n):
                {
                    if (n.left is Leaf && n.right is Leaf && n.depth > 4) {
                        left = (Leaf)(n.left);
                        right = (Leaf)(n.right);
                        return true;
                    }
                    return false;
                }

                default:
                    return false;
            }
        }

        private static bool Split(Tree t, bool b) {

            switch(t) {
                case (Leaf n):
                {
                    if (n.value >= 10) {
                        int r;
                        int d = Math.DivRem(n.value, 2, out r);
                        Leaf newleft = new Leaf() {value = d};
                        Leaf newright = new Leaf() {value = d + r};

                        Node tt = new Node() {left = newleft, right = newright};
                        newleft.parent = tt;
                        newright.parent = tt;
                        newleft.depth = n.depth + 1;
                        newright.depth = n.depth + 1;
                        newleft.right_leaf = newright;
                        newright.left_leaf = newleft;
                        newleft.left_leaf = n.left_leaf;
                        if (n.left_leaf != null) n.left_leaf.right_leaf = newleft;
                        newright.right_leaf = n.right_leaf;
                        if (n.right_leaf != null) n.right_leaf.left_leaf = newright;

                        tt.parent = t.parent;
                        tt.depth = n.depth;

                        
                        Node par = (Node)(n.parent);
                        if (par.left == n) { 
                            par.left = tt;
                        } else {
                            par.right = tt;
                        }
                        return true;                        
                    }
                    else
                        return false;
                }

                case (Node n):
                {
                    bool bb = Split(n.left, b);
                    if (!bb) bb = Split(n.right, b);
                    return bb;
                }

                default:
                    return false;
            }
        }

        private static Leaf first_leaf = null;
        private static Leaf last_leaf = null;

        private static Tree ParseNumber(string s, int pos, out int newpos, int depth) {
            newpos = pos;
            
            if (s[pos] == '[') {
                pos += 1;
                var l = ParseNumber(s, pos, out newpos, depth+1);
                pos = newpos + 1; // should be a , - maybe we should check?
                var r = ParseNumber(s, pos, out newpos, depth+1);
                newpos += 1;
                var res = new Node() {left = l, right = r, depth = depth };
                l.parent = res;
                r.parent = res;
                return res;
            }

            if (Char.IsDigit(s[pos])) {
                int n = 0;
                while(Char.IsDigit(s[pos])) {
                    n = n * 10;
                    n += ParseDigit(s[pos]);
                    pos += 1;
                }
                newpos = pos;

                Leaf me = new Leaf() { value = n, depth = depth };
                if (first_leaf == null) first_leaf = me;
                me.left_leaf = last_leaf;
                if (last_leaf != null) last_leaf.right_leaf = me;
                last_leaf = me;

                return me;
            }

            return null;
        }

        private static int ParseDigit(char v)
        {
            return v - '0';
        }

        private static Tree Copy(Tree t) {
            last_leaf = null;
            first_leaf = null;
            return AuxCopy(t, null);
        }

        private static Tree AuxCopy(Tree t, Node parent) 
        {
            switch (t) {
                case (Leaf l) : {
                    Leaf nl = new Leaf() {value = l.value, parent = parent, depth = l.depth};
                    if (first_leaf == null) first_leaf = nl;
                    nl.left_leaf = last_leaf;
                    if (last_leaf != null) last_leaf.right_leaf = nl;
                    last_leaf = nl;
                    return nl;
                }

                case (Node n) : {
                    Node nn = new Node() { parent = parent, depth = n.depth };
                    Tree left = AuxCopy(n.left, nn);
                    Tree right = AuxCopy(n.right, nn);
                    nn.left = left;
                    nn.right = right;
                    return nn;
                }

                default:
                    return null;
            }
        }
    }
}
