using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day15
{
    internal class HashSolution2 : HashBase
    {
        protected override object DoSolve()
        {
            var input = ReadInputFromFile();
            var instructions = input.Select(x => new Instruction(x)).ToList();
            var boxes = Enumerable.Range(0, 256).Select(x => new List<Lens>()).ToList();
            foreach (var instruction in instructions)
            {
                if (instruction.Operator == '=')
                {
                    if (boxes[instruction.Box].FindIndex(x => x.Label == instruction.Label) is var foundItemIndex and > -1)
                    {
                        boxes[instruction.Box][foundItemIndex].Value = instruction.Value;
                    }
                    else
                    {
                        boxes[instruction.Box].Add(new Lens { Label = instruction.Label, Value = instruction.Value });
                    }
                }
                else if (instruction.Operator == '-')
                {
                    for (int i = 0; i < boxes[instruction.Box].Count; i++)
                    {
                        if (boxes[instruction.Box][i].Label == instruction.Label)
                        {
                            boxes[instruction.Box].RemoveAt(i);
                            break;
                        }
                    }
                }   
            }
            return boxes.Sum(x => GetFocusingPower(x, boxes.IndexOf(x)));
        }

        private int GetFocusingPower(List<Lens> lenses, int boxNumber)
        {
            var power = 0;
            for (int i = 0; i < lenses.Count; i++)
            {
                power += (boxNumber + 1) * (i + 1) * lenses[i].Value;
            }
            return power;
        }
    }

    public class Instruction
    {
        private static readonly Regex _regex = new(@"^(?<label>\w+)(?<instruction>[=-])(?<value>\d)?", RegexOptions.Compiled);

        public Instruction(string instruction)
        {
            var match = _regex.Match(instruction);
            Label = match.Groups["label"].ToString();
            Operator = match.Groups["instruction"].ToString()[0];
            Box = HashBase.Hash(Label);
            Value = -1;
            if (int.TryParse(match.Groups["value"].ToString(), out var value))
            {
                Value = value;
            }
        }

        public string Label { get; }
        public int Box { get; }
        public char Operator { get; }
        public int Value { get; }
    }

    public class Lens
    {
        public string Label { get; set; }
        public int Value { get; set; }
    }
}
