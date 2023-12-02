using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day02
{
    public abstract class CubeBase : BaseSolution<IEnumerable<CubeGame>>
    {
        internal override IEnumerable<CubeGame> ReadInputFromFile()
        {
            var lines = File.ReadLines("./Puzzles/2023/Day02/Input.txt");
            return lines.Select(l => new CubeGame(l));
        }
    }

    public class CubeGame
    {
        private static readonly Regex _gameParserRegex = new("Game (?<GameId>[0-9]{1,3}):", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public CubeGame(string input)
        {
            var match = _gameParserRegex.Match(input);
            if (!match.Success)
            {
                throw new ArgumentException("Input is not a valid cube game", nameof(input));
            }
            if (int.TryParse(match.Groups["GameId"].Value, out var gameId))
            {
                GameID = gameId;
                CubeSets = input.Substring(match.Index + match.Length).Split(";").Select(s => _buildCubeset(s));
            }
        }

        public int GameID { get; set; }
        public IEnumerable<Dictionary<Color, int>> CubeSets { get; set; }

        private Dictionary<Color, int> _buildCubeset(string setInput)
        {
            return setInput.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(c => new Cube(c.Trim())).ToDictionary(k => k.Color, k => k.Count);
        }
    }

    public class Cube
    {
        private static readonly Regex _cubeParserRegex = new("(?<CubeCount>[0-9]{1,}) (?<Color>red|green|blue)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public Cube(string cubeInput)
        {
            var match = _cubeParserRegex.Match(cubeInput);
            if (!match.Success)
            {
                throw new ArgumentException("Input is not a valid cube", nameof(cubeInput));
            }
            if (int.TryParse(match.Groups["CubeCount"].Value, out var cubeCount))
            {
                Count = cubeCount;
            }
            if (Enum.TryParse(typeof(Color), match.Groups["Color"].Value.ToLower(), out var colorParsed) && colorParsed is Color color)
            {
                Color = color;
            }
        }

        public int Count { get; set; }
        public Color Color { get; set; }
    }
    
    public enum Color
    {
        blue,
        red,
        green
    }
}
