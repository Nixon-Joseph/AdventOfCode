using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day01
{
    internal class CalibrationSolution2 : CalibrationBase
    {
        private static readonly Regex _digitRegex = new("(one|two|three|four|five|six|seven|eight|nine)|([1-9]{1})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override object Solve()
        {
            var input = ReadInputFromFile();
            // find the first and last digit in each input line, and append them together.
            var result = 0;
            foreach (var line in input)
            {
                if (string.IsNullOrWhiteSpace(line)) { continue; }

                var soloMatches = new List<string>();
                var nextStart = 0;
                while (_digitRegex.IsMatch(line, nextStart))
                {
                    var nextMatch = _digitRegex.Match(line, nextStart);
                    soloMatches.Add(nextMatch.Value);
                    nextStart = nextMatch.Index + (nextMatch.Length > 1 ? nextMatch.Length - 1 : nextMatch.Length);
                }
                var first = GetDigitFromInput(soloMatches[0]);
                var last = GetDigitFromInput(soloMatches[^1]);
                if (int.TryParse($"{first}{last}", out var calibrationValue))
                {
                    result += calibrationValue;
                }
            }
            return result;
        }

        private char GetDigitFromInput(string input)
        {
            if (int.TryParse(input, out var digit))
            {
                return digit.ToString()[0];
            }
            return input.ToLower() switch
            {
                "one" => '1',
                "two" => '2',
                "three" => '3',
                "four" => '4',
                "five" => '5',
                "six" => '6',
                "seven" => '7',
                "eight" => '8',
                "nine" => '9',
                _ => '0'
            };
        }
    }
}
