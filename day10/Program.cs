using System;
using System.IO;
using System.Collections.Generic;

namespace day10
{


    class Program
    {

        static int roundbrackpoints = 3;
        static int squarebracketpoints = 57;
        static int curleybracketspoints = 1197;
        static int anglebracketspoints = 25137;
        
        static void Main(string[] args)
        {
            Part2(args);
        }
    
        static void Part1(string[] args) {

            int totalScore = 0;

            var alllines = File.ReadAllLines(args[0]);
            for(int i = 0; i < alllines.Length; i++) {

                Stack<char> opens = new Stack<char>();
                bool error = false;
                int score = 0;
                char c = ' ';
                char top = ' ';
                for(int j = 0; j < alllines[i].Length; j++) {
                    c = alllines[i][j];
                    if (c == '(' || c == '{' || c == '[' || c == '<') {
                        opens.Push(c);
                    }

                    if (c == '}') {
                        if (opens.Count == 0) {
                            score = curleybracketspoints;
                            Console.Out.WriteLine($"}} but empty stack."); 
                            error = true;
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '{') {
                                score = curleybracketspoints;
                                Console.Out.WriteLine($"}} but found {top}");
                                error = true;
                                break;
                            }
                        }
                    }
                    if (c == ']') {
                        if (opens.Count == 0) {
                            score = squarebracketpoints;
                            Console.Out.WriteLine($"] but empty stack."); 
                            error = true;
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '[') {
                                score = squarebracketpoints;
                                Console.Out.WriteLine($"] but found {top}");
                                
                                error = true;
                                break;
                            }
                        }
                    }
                    if (c == '>') {
                        if (opens.Count == 0) {
                            score = anglebracketspoints;
                            Console.Out.WriteLine($"> but empty stack."); 
                            error = true;
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '<') {
                                score = anglebracketspoints;
                                Console.Out.WriteLine($"> but found {top}");
                                error = true;
                                break;
                            }
                        }
                    }
                    if (c == ')') {
                        if (opens.Count == 0) {
                            score = roundbrackpoints;
                            Console.Out.WriteLine($") but empty stack.");
                            error = true; 
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '(') {
                                score = roundbrackpoints;
                                Console.Out.WriteLine($") but found {top}");
                                error = true;
                                break;
                            }
                        }
                    }
                }
                if (error)
                    Console.Out.WriteLine($"Line {i} has score {score} but we've got a stack containing {opens.Count} items. c = {c} top = {top}");
                totalScore += score;
            }

            Console.Out.WriteLine($"Total score = {totalScore}");

        }


        static void Part2(string[] args) {
            List<long> scores = new List<long>();
            
            var alllines = File.ReadAllLines(args[0]);
            for(int i = 0; i < alllines.Length; i++) {

                Stack<char> opens = new Stack<char>();
                bool error = false;
                int score = 0;
                char c = ' ';
                char top = ' ';
                for(int j = 0; j < alllines[i].Length; j++) {
                    c = alllines[i][j];
                    if (c == '(' || c == '{' || c == '[' || c == '<') {
                        opens.Push(c);
                    }

                    if (c == '}') {
                        if (opens.Count == 0) {
                            score = curleybracketspoints;
                            Console.Out.WriteLine($"}} but empty stack."); 
                            error = true;
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '{') {
                                score = curleybracketspoints;
                                Console.Out.WriteLine($"}} but found {top}");
                                error = true;
                                break;
                            }
                        }
                    }
                    if (c == ']') {
                        if (opens.Count == 0) {
                            score = squarebracketpoints;
                            Console.Out.WriteLine($"] but empty stack."); 
                            error = true;
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '[') {
                                score = squarebracketpoints;
                                Console.Out.WriteLine($"] but found {top}");
                                
                                error = true;
                                break;
                            }
                        }
                    }
                    if (c == '>') {
                        if (opens.Count == 0) {
                            score = anglebracketspoints;
                            Console.Out.WriteLine($"> but empty stack."); 
                            error = true;
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '<') {
                                score = anglebracketspoints;
                                Console.Out.WriteLine($"> but found {top}");
                                error = true;
                                break;
                            }
                        }
                    }
                    if (c == ')') {
                        if (opens.Count == 0) {
                            score = roundbrackpoints;
                            Console.Out.WriteLine($") but empty stack.");
                            error = true; 
                            break;
                        } else {
                            top = opens.Pop();
                            if (top != '(') {
                                score = roundbrackpoints;
                                Console.Out.WriteLine($") but found {top}");
                                error = true;
                                break;
                            }
                        }
                    }
                }
                if (!error && opens.Count > 0) {
                    string s = "";
                    char cc;
                    long localscore = 0;
                    while(opens.TryPop(out cc)) {
                        char nc = (cc == '(' ? ')' : (cc == '[' ? ']' : (cc == '<' ? '>' : '}')));
                        long scoreinc = (cc == '(' ? 1 : (cc == '[' ? 2 : (cc == '<' ? 4 : 3)));
                        s += $"{nc}";
                        localscore = localscore * 5;
                        localscore += scoreinc;
                    }

                    Console.Out.WriteLine($"{alllines[i]} completed by {s} score is {score}");
                    scores.Add(localscore);
                }
            }

            var asarr = scores.ToArray();
            Array.Sort(asarr);
            var mid = asarr[asarr.Length/2];
            Console.Out.WriteLine($"Middle score is {mid}");

        }
    }
}

/*
[({(<(())[]>[[{[]{<()<>> - Complete by adding }}]])})].
[(()[<>])]({[<{<<[]>>( - Complete by adding )}>]}).
(((({<>}<{<{<>}{[]{[]{} - Complete by adding }}>}>)))).
{<[[]]>}<{[{[{[]{()[[[] - Complete by adding ]]}}]}]}>.
<{([{{}}[<[[[<>{}]]]>[]] - Complete by adding ])}>.

}}]])})] - 288957 total points.
)}>]}) - 5566 total points.
}}>}>)))) - 1480781 total points.
]]}}]}]}> - 995444 total points.
])}> - 294 total points.
*/
