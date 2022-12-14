using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode.Puzzles.Day10
{
    public class CathodeSolution : BaseSolution<IEnumerable<(Operation op, int signal)>>
    {
        private static readonly Regex OperationRegex = new("^(?<op>noop|addx){1}[ ]{0,}(?<signal>[0-9-]{0,})?$", RegexOptions.Compiled);

        internal override IEnumerable<(Operation op, int signal)> ReadInputFromFile()
        {
            var allLines = File.ReadAllLines("./Puzzles/Day10/Input.txt");
            var ops = new List<(Operation, int)>(); // (op, signal)
            foreach (var line in allLines)
            {
                var match = OperationRegex.Match(line);
                if (match.Success)
                {
                    ops.Add((Enum.Parse<Operation>(match.Groups["op"].Value, true), !string.IsNullOrEmpty(match.Groups["signal"].Value) ? int.Parse(match.Groups["signal"].Value) : -1));
                }
            }
            return ops;
        }

        public override object Solve()
        {
            var cycle = 1;
            var x = 1;
            var signalStrengths = new List<int>();
            var ops = ReadInputFromFile();
            var lineOutput = "";
            char pixel;
            var handleOuputFunc = () =>
            {
                var hpos = cycle % 40 - 1;
                pixel = '.';
                if (hpos >= (x - 1) && hpos <= (x + 1))
                    pixel = '#';
                lineOutput += pixel;
                if (cycle % 40 == 0)
                {
                    signalStrengths.Add(x * cycle);
                    Console.WriteLine(lineOutput);
                    lineOutput = "";
                }
            };
            foreach (var (op, signal) in ops)
            {
                handleOuputFunc();
                switch (op)
                {
                    case Operation.Addx:
                        cycle++;
                        handleOuputFunc();
                        x += signal;
                        break;
                    case Operation.Noop: break;
                }
                cycle++;
            }
            return "";
        }
    }

    public enum Operation
    {
        Noop,
        Addx
    }
}
