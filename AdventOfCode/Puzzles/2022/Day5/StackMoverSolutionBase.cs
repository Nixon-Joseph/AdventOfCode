using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Day5
{
    public abstract class StackMoverSolutionBase : BaseSolution<CrateStacks>
    {
        private static readonly Regex CrateBottomFinderRegex = new Regex("(?: [1-9] [ ]?)+", RegexOptions.Compiled);

        internal override CrateStacks ReadInputFromFile()
        {
            var fileLines = File.ReadAllLines("./Puzzles/Day5/Input.txt").ToList();
            var crateBottomIndex = fileLines.FindIndex(x => CrateBottomFinderRegex.IsMatch(x));
            var stackCount = int.Parse(fileLines[crateBottomIndex].Split(" ", StringSplitOptions.RemoveEmptyEntries).Last());
            var stacks = GetStackList(stackCount);
            for (var i = crateBottomIndex - 1; i >= 0; i--)
            {
                var crateLine = fileLines[i];
                var crateIndex = 0;
                while (crateIndex < stackCount)
                {
                    var crateText = crateLine.Substring(crateIndex * 4, 3).Trim();
                    if (crateText.Length == 3)
                        stacks.ElementAt(crateIndex).Push(crateText[1]);
                    crateIndex++;
                }
            }
            foreach(var fileLine in fileLines.Skip(crateBottomIndex + 1)) {
                stacks.AddInstruction(fileLine);
            }
            return stacks;
        }

        private static CrateStacks GetStackList( int stackCount ) {
            var looper = 0;
            var stacks = new CrateStacks();
            while (looper < stackCount)
            {
                stacks.Add(new Stack<char>());
                looper++;
            }
            return stacks;
        }

        protected string GetStackTops(CrateStacks stacks)
        {
            var stackTops = new List<char>();
            foreach (var stack in stacks)
            {
                if (stack.Count > 0)
                    stackTops.Add(stack.Peek());
                else
                    stackTops.Add(' ');
            }
            return string.Join("", stackTops);
        }
    }

    public class CrateStacks : List<Stack<char>>
    {
        public List<Instruction> Instructions { get; set; } = new();
        public void AddInstruction(string instruction)
        {
            Instructions.Add(new Instruction(instruction));
        }

        public class Instruction
        {
            private static readonly Regex InstructionReaderRegex = new("^move (?<moveCount>[0-9]{1,2}) from (?<moveFrom>[1-9]) to (?<moveTo>[1-9])", RegexOptions.Compiled);
            public int MoveCount { get; set; }
            public int MoveFrom { get; set; }
            public int MoveTo { get; set; }

            public Instruction(string instruction) {
                if( string.IsNullOrWhiteSpace( instruction ) )
                    return;
                var match = InstructionReaderRegex.Match(instruction);
                MoveCount = int.Parse(match.Groups["moveCount"].Value);
                MoveFrom = int.Parse(match.Groups["moveFrom"].Value);
                MoveTo = int.Parse(match.Groups["moveTo"].Value);
            }
        }
    }
}
