using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day16
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1(args);
        }

        private static void Part1(string[] args)
        {
            string s = File.ReadAllLines(args[0])[0];

            string bits = "";
            foreach (char c in s)
            {
                bits = bits + BitsFromChar(c);
            }

            Console.Out.WriteLine($"{bits}");
            int progress;
            long v = ExtractPacket2(bits, 0, out progress);
            Console.Out.WriteLine($"Version sum is {v}");
        }

        //
        // This is the Part 1 version, which returns the version number, and
        // sums the versions of sub packets if necessary.
        private static long ExtractPacket(string bits, int start, out int progress)
        {
            long version = ToLong(bits, start, 3);
            string type = bits.Substring(start + 3, 3);

            if (type == "100")
            { // Literal.
                long l = 0;
                progress = start + 6;
                for (int j = start+6; true; j += 5)
                {
                    progress += 5;
                    l = l * 16;
                    l += ToLong(bits, j + 1, 4);
                    if (bits[j] == '0')
                        break;
                }
                Console.Out.WriteLine($"{l}");
                return version;
            }

            else
            { // operator type
                long len = -1;
                if (bits[start+6] == '0')
                { // 15 bit length
                    len = ToLong(bits, start + 7, 15);
                    progress = start + 7 + 15;
                    while (progress < (start + 7 + 15 + len)) {
                        version += ExtractPacket(bits, progress, out progress);
                    }
                    return version;
                }
                else
                { // 11 bit length
                    len = ToLong(bits, start + 7, 11);
                    progress = start + 7 + 11;
                    // here, len is number of packets to expect.
                    for(long i = 0; i < len; i++) {
                        version += ExtractPacket(bits, progress, out progress);
                    }
                    return version;
                }
            }
        }

        //
        // This is Part 2, which collects the numbers from sub packets, or returns the
        // number for a type 4 packet.
        // Then inteprets the type to perform an operation.
        private static long ExtractPacket2(string bits, int start, out int progress)
        {
            long version = ToLong(bits, start, 3);
            long type = ToLong(bits, start+3, 3); //.Substring(start + 3, 3);

            if (type == 4)
            { // Literal.
                long l = 0;
                progress = start + 6;
                for (int j = start+6; true; j += 5)
                {
                    progress += 5;
                    l = l * 16;
                    l += ToLong(bits, j + 1, 4);
                    if (bits[j] == '0')
                        break;
                }
                Console.Out.WriteLine($"{l}");
                return l;
            }

            else
            { // operator type
                long len = -1;
                List<long> numbers = new List<long>();
                if (bits[start+6] == '0')
                { // 15 bit length
                    len = ToLong(bits, start + 7, 15);
                    progress = start + 7 + 15;
                    while (progress < (start + 7 + 15 + len)) {
                        long n = ExtractPacket2(bits, progress, out progress);
                        numbers.Add(n);
                    }
                }
                else{ // 11 bit length
                    len = ToLong(bits, start + 7, 11);
                    progress = start + 7 + 11;
                    // here, len is number of packets to expect.
                    for(long i = 0; i < len; i++) {
                        long n = ExtractPacket2(bits, progress, out progress);
                        numbers.Add(n);
                    }
                }

                if (type == 0) {
                    return numbers.Sum();
                } else if (type == 1) {
                    long res = 1;
                    foreach(var x in numbers) res *= x;
                    return res; 
                } else if (type == 2) {
                    return numbers.Min();
                } else if (type == 3) {
                    return numbers.Max();
                } else if (type == 5) {
                    return numbers[0] > numbers[1] ? 1 : 0;
                } else if (type == 6) {
                    return numbers[0] < numbers[1] ? 1 : 0;
                } else if (type == 7) {
                    return numbers[0] == numbers[1] ? 1 : 0;
                } else {
                    throw new Exception($"Didn't find the operation for {version}");
                }

                Console.WriteLine($"Operator packet with number of bits {len}");
            }
            return -1000;
        }
    

        static long ToLong(string bits, int v1, int v2)
        {
            long res = 0;
            for(int i = v1; i < v1+v2; i++) {
                res = res * 2;
                if (bits[i] == '1') res += 1;
            }
            return res;
        }

        static byte FromChar(char c) {
            if (Char.IsDigit(c)) {
                return (byte)(c - '0');
            }

            if (c == 'A') return 10;
            if (c == 'B') return 11;
            if (c == 'C') return 12;
            if (c == 'D') return 13;
            if (c == 'E') return 14;
            if (c == 'F') return 15;
            
            throw new Exception("Not a hex expected character.");
        }

        static string BitsFromChar(char c) {
            
            if (c == '0') return "0000";
            if (c == '1') return "0001";
            if (c == '2') return "0010";
            if (c == '3') return "0011";
            if (c == '4') return "0100";
            if (c == '5') return "0101";
            if (c == '6') return "0110";
            if (c == '7') return "0111";
            if (c == '8') return "1000";
            if (c == '9') return "1001";
            
            if (c == 'A') return "1010";
            if (c == 'B') return "1011";
            if (c == 'C') return "1100";
            if (c == 'D') return "1101";
            if (c == 'E') return "1110";
            if (c == 'F') return "1111";
            return "";
        }
    }
}
