using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace day24
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1(args);
        }

        static List<Func<(Dictionary<string,long>,Stack<long>),(Dictionary<string,long>,Stack<long>)>> code
                = new List<Func<(Dictionary<string, long>, Stack<long>), (Dictionary<string, long>, Stack<long>)>>();
        static List<string> opaccs = new List<string>();
        static long step = 1000000;

        private static void Part1(string[] args)
        {
            ThreadPool.SetMaxThreads(16,1);

            var alllines = File.ReadAllLines(args[0]);
            code = new List<Func<(Dictionary<string, long>, Stack<long>), (Dictionary<string, long>, Stack<long>)>>();
            opaccs = new List<string>();

            string[] ops = new string[alllines.Length];
            Arg[][] allargs = new Arg[alllines.Length][];
            int q = 0;
            foreach(var s in alllines) {
                string op;
                var opargs = ParseLine(s, out op);
                ops[q] = op;
                allargs[q] = opargs;
                foreach(var a in opargs) {
                    if (!a.literal && !opaccs.Contains(a.acc)) {
                        opaccs.Add(a.acc);
                    }
                }
            }

            bool[] include = new bool[alllines.Length];
            Dictionary<string, Probe> ans = new Dictionary<string, Probe>();
            foreach(var s in opaccs) {
                ans.Add(s, new Probe(true));
            }

            int ert = 0;
            foreach(var s in alllines) {
                string op;
                var opargs = ParseLine(s, out op);
                //if (Analyse(op, opargs, ans)) {
                    var f = Compile(op, opargs, opaccs);
                    code.Add(f);
                    ert+=1;
                //}
            }
            Console.WriteLine($"Started with {alllines.Length} lines, down to {count}");
            Console.In.ReadLine();

            long l = 99897039000000;
            while (l > 9999999999999) {
                lock(lockObj){
                    count = 0;
                }

                ranges.Push(l);
                ranges.Push(l-step);
                ranges.Push(l-(2*step));
                ranges.Push(l-(3*step));
                ranges.Push(l-(4*step));
                ranges.Push(l-(5*step));
                ranges.Push(l-(6*step));
                ranges.Push(l-(7*step));
                ranges.Push(l-(8*step));
                ranges.Push(l-(9*step));
                ranges.Push(l-(10*step));
                ranges.Push(l-(11*step));
                ranges.Push(l-(12*step));
                ranges.Push(l-(13*step));
                l = l-(14*step);

                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);
                //ThreadPool.QueueUserWorkItem(DoWork, null);

                Task<bool>[] tasks = new Task<bool>[14];
                for(int tt = 0; tt < 14; tt++)
                    tasks[tt] = FindByWork();
                
                Console.WriteLine("14 new tasks started - waiting for them to end.");
                Task.WaitAll(tasks);

                //while (count < 14) 
                //    Thread.Sleep(50);

                if (found)
                    break;
            }
/*
            Stack<long> inputs = new Stack<long>();
            Dictionary<string,long> accs = new Dictionary<string, long>();
            for(long k = 100000000000000; k > 9999999999999; k--) {
            
                if (k % 1000000 == 0) Console.WriteLine($"k={k}");
                if ( CreateStack(k, out inputs)) {
                    //foreach(var s in alllines) {
                    //    string op;
                    //    var opargs = ParseLine(s, out op);
                    //    Interpret(op, opargs, accs, inputs);
                    //}
                    foreach(var symb in opaccs)
                        accs.Add(symb,0);
                    
                    int c = 0;
                    foreach(var func in code) {
                        Console.In.ReadLine();
                        foreach(var s in opaccs) {
                            Console.Write($"{s} = {accs[s]} ");
                        }
                        Console.WriteLine("");
                        Console.WriteLine($"{alllines[c]}");
                        func((accs, inputs));
                        c++;
                    }

                    if (accs["z"] == 0) {
                        Console.WriteLine($"Value is {k}");
                        break;
                    }

                    accs = new Dictionary<string, long>();
                } 
            } 

            foreach((string nm, long v) in accs) {
                Console.Out.WriteLine($"{nm} = {v}");
            } */
            
        }

        private static Object lockObj = new Object();
        private static long value = 0;
        private static bool found = false;
        private static Stack<long> ranges = new Stack<long>();

        private static int count = 0;

        private static Task<bool> FindByWork() {
            return Task.Run( () => {
                DoWork(null);
                return true;
            });
        }

        private static void DoWork(Object stateinfo) { //long start, long end) {
            long start, end;
            lock(lockObj) {
                start = ranges.Pop();
                end = start - step;
            }

            Console.WriteLine($"Doing {start} to {end}");
            Stack<long> inputs = new Stack<long>();
            Dictionary<string,long> accs = new Dictionary<string, long>();
            for(long k = start; k > end; k--) {
                if ( CreateStack(k, out inputs)) {
                    //foreach(var s in alllines) {
                    //    string op;
                    //    var opargs = ParseLine(s, out op);
                    //    Interpret(op, opargs, accs, inputs);
                    //}
                    foreach(var symb in opaccs)
                        accs.Add(symb,0);
                    
                    foreach(var func in code) {
                        func((accs, inputs));
                    }

                    //Console.WriteLine($"Value is {k} z is {accs["z"]}");
                    if (accs["z"] == 0) {
                        Console.WriteLine($"Value is {k}");
                        lock(lockObj) {

                            long v = accs["z"];
                            if (v > value) {
                                value = v;
                                found = true;
                            }
                            count += 1;
                            return;
                        }
                    }

                    //k = k - (accs["z"] + 1);
                    accs = new Dictionary<string, long>();
                }
            }
            lock(lockObj) {
                count += 1;
            }
        }

        private static bool CreateStack(long k, out Stack<long> inputs)
        {
            inputs = new Stack<long>();
            long n = k;
            long m = 9;
            while(n > 0) {
                n = Math.DivRem(n, 10, out m);
                if (m == 0) return false;
                inputs.Push(m);
            }
            return true;
        }

// 

        private static Func<(Dictionary<string,long>,Stack<long>), (Dictionary<string,long>,Stack<long>)> 
            Compile(string op, Arg[] args, List<string> accs)
        {
            if (args[0].literal)
                throw new Exception($"First arg not a literal");
            
            if (!accs.Contains(args[0].acc))
                accs.Add(args[0].acc);

            if (args.Length == 2 && !args[1].literal && !accs.Contains(args[1].acc))
                accs.Add(args[1].acc);
            
            string arg1 = args[0].acc; // always a string.
            switch (op) {

                case "inp": {
                    return p => {
                        (Dictionary<string,long> st, Stack<long> inps) = p; st[arg1] = inps.Pop(); return (st,inps); };
                }
                case "add": {
                    if (args[1].literal) {
                        long arg2 = args[1].value;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            v += arg2;
                            st[arg1] = v;
                            return (st,inps);
                        };
                    }
                    else {
                        string arg2 = args[1].acc;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            v += st[arg2];
                            st[arg1] = v;
                            return (st,inps);
                        };
                    }
                }
                case "mul": {
                    if (args[1].literal) {
                        long arg2 = args[1].value;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            v *= arg2;
                            st[arg1] = v;
                            return (st,inps);
                        };
                    }
                    else {
                        string arg2 = args[1].acc;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            v *= st[arg2];
                            st[arg1] = v;
                            return (st,inps);
                        };
                    }
                }
                case "div": {
                    if (args[1].literal) {
                        long arg2 = args[1].value;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            long m;
                            v = Math.DivRem(v, arg2, out m);
                            st[arg1] = v;
                            return (st,inps);
                        };
                    }
                    else {
                        string arg2 = args[1].acc;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            long m;
                            v = Math.DivRem(v, st[arg2], out m);
                            st[arg1] = v;
                            return (st,inps);
                        };
                    }
                }
                case "mod": {
                    if (args[1].literal) {
                        long arg2 = args[1].value;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            long m;
                            v = Math.DivRem(v, arg2, out m);
                            st[arg1] = m;
                            return (st,inps);
                        };
                    }
                    else {
                        string arg2 = args[1].acc;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            long m;
                            v = Math.DivRem(v, st[arg2], out m);
                            st[arg1] = m;
                            return (st,inps);
                        };
                    }
                }
                case "eql": {
                    if (args[1].literal) {
                        long arg2 = args[1].value;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            st[arg1] = (v == arg2) ? 1 : 0;
                            return (st,inps);
                        };
                    }
                    else {
                        string arg2 = args[1].acc;
                        return p => {
                            (Dictionary<string,long> st, Stack<long> inps) = p;
                            long v = st[arg1];
                            long m = st[arg2];
                            st[arg1] = (v == m) ? 1 : 0;
                            return (st,inps);
                        };
                    }
                }                
                default:
                    throw new Exception($"what's {op}");
            }
        }

        private static void Interpret(string op, Arg[] args, Dictionary<string, long> accs, Stack<long> inputs)
        {
            if (args[0].literal)
                throw new Exception($"First arg not a literal");
            
            if (!accs.ContainsKey(args[0].acc))
                accs.Add(args[0].acc, 0);

            if (args.Length == 2 && !args[1].literal && !accs.ContainsKey(args[1].acc))
                accs.Add(args[1].acc, 0);
            
            switch (op) {

                case "inp": {
                    accs[args[0].acc] = inputs.Pop();
                }
                break;
                case "add": {
                    long v = accs[args[0].acc];
                    v += args[1].literal ? args[1].value : accs[args[1].acc];
                    accs[args[0].acc] = v;
                }
                break;
                case "mul": {
                    long v = accs[args[0].acc];
                    v *= args[1].literal ? args[1].value : accs[args[1].acc];
                    accs[args[0].acc] = v;
                }
                break;
                case "div": {
                    long v = accs[args[0].acc];
                    long w =  args[1].literal ? args[1].value : accs[args[1].acc];
                    long rem;
                    accs[args[0].acc] = Math.DivRem(v,w, out rem);
                }
                break;
                case "mod": {
                    long v = accs[args[0].acc];
                    long w =  args[1].literal ? args[1].value : accs[args[1].acc];
                    long rem;
                    Math.DivRem(v,w, out rem);
                    accs[args[0].acc] = rem; 
                }
                break;
                case "eql": {
                    long v = accs[args[0].acc];
                    long w =  args[1].literal ? args[1].value : accs[args[1].acc];
                    accs[args[0].acc] = (v == w) ? 1 : 0;
                }
                break;
                
                default:
                    throw new Exception($"what's {op}");
            }
        }

        static Arg[] ParseLine(string s, out string op) {
            op = s.Substring(0,3);
            var args = s.Substring(4).Split(' ',StringSplitOptions.RemoveEmptyEntries);
            Arg[] argsObj = new Arg[args.Length];
            for(int i = 0; i < args.Length; i++) {
                argsObj[i] = new Arg(args[i]);
            }
            return argsObj;
        }

        static bool Analyse(string op, Arg[] opargs, Dictionary<string, Probe> ans) {
            string act = opargs[0].acc; // must be a literal.
            switch (op) {
                case "add": {
                    if (opargs[1].literal && opargs[1].value == 0)
                        return false;
                    if (!opargs[1].literal && ans[opargs[1].acc].isZero)
                        return false;
                    
                    if (ans[act].isZero) ans[act].isZero = false;
                    return true;
                }
                case "mul": {
                    if (ans[act].isZero)
                        return false;
                    if (!ans[act].isZero && opargs[1].literal && opargs[1].value == 0) {
                        ans[act].isZero = true;
                        return true;
                    }
                    if (!ans[act].isZero && !opargs[1].literal && ans[opargs[1].acc].isZero) {
                        ans[act].isZero = true;
                        return true;
                    }

                    return true;
                    
                }
                case "div":
                    if (ans[act].isZero)
                        return false;
                    if (opargs[0].literal && opargs[0].value == 1)
                        return false;
                    return true;
                case "mod":
                    if (ans[act].isZero)
                        return false;
                    return true;
                case "eql":
                    return true;
                case "inp": {
                    ans[act].isZero = false;
                    return true;
                }
            }
            ans[act].isZero = false;
            return true;

        }
    }
}
