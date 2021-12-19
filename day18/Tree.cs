using System;
using System.Collections.Generic;

namespace day18
{

    //
    // A Tree is a Node with two branches, or a Leaf with an integer value.
    // A Tree item knows its parent and depth.
    // A Leaf has links to left and right adjacent leaf items so it can easily find them.
    public abstract class Tree {
        public abstract void print();
        public int depth {get; set;}

        public abstract void setDepth(int d);

        public Tree parent {get; set;}

        public T applyT<T,U>(Func<Tree,U,T> app, U u) {
            return app(this, u);
        }
    }

    public class NullTree : Tree
    {
        public override void print()
        {
            throw new NotImplementedException();
        }

        public override void setDepth(int d)
        {
            throw new NotImplementedException();
        }
    }

    public class Node : Tree {
        public Tree left {get; set; }
        public Tree right {get; set; }

        public override void print()
        {
            Console.Out.Write($"[");
            left.print();
            Console.Out.Write(",");
            right.print();
            Console.Out.Write("]");
        }

        public override void setDepth(int d)
        {
            this.depth = d;
            left.setDepth(d+1);
            right.setDepth(d+1);
        }

    }

    public class Leaf : Tree {
        public int value {get; set; }

        public Leaf left_leaf {get; set;}
        public Leaf right_leaf {get; set;}
        
        public override void print()
        {
            Console.Write($"{value}");
        }

        public override void setDepth(int d)
        {
            this.depth = d;
        }
    }
}
