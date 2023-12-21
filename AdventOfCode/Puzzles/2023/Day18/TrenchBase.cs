using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day18
{
    internal abstract class TrenchBase : BaseSolution<List<DigInstruction>>
    {
        private static readonly Regex _regex = new Regex(@"^(?<direction>[UDLR]) (?<distance>[\d]+) \((?<hex>#[\d\w]{6})\)$");

        internal override List<DigInstruction> ReadInputFromFile()
        {
            var lines = ReadFileAsLines(@".\Puzzles\2023\Day18\Input.txt");
            var result = new List<DigInstruction>();
            foreach (var line in lines)
            {
                var match = _regex.Match(line);
                result.Add(new DigInstruction
                {
                    ColorHex = match.Groups["hex"].ToString(),
                    Direction = match.Groups["direction"].ToString() switch
                    {
                        "U" => Direction.Up,
                        "D" => Direction.Down,
                        "L" => Direction.Left,
                        "R" => Direction.Right,
                    },
                    Distance = int.Parse(match.Groups["distance"].ToString())
                });
            }
            return result;
        }
    }

    public class DigInstruction
    {
        public Direction Direction { get; set; }
        public int Distance { get; set; }
        public string ColorHex { get; set; }
    }
}
