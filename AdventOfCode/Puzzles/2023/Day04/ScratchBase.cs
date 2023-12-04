using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day04
{
    internal abstract class ScratchBase : BaseSolution<List<ScratchCard>>
    {
        internal override List<ScratchCard> ReadInputFromFile()
        {
            var lines = File.ReadLines("./Puzzles/2023/Day04/Input.txt");
            return lines.Select(l => new ScratchCard(l)).ToList();
        }
    }

    internal class ScratchCard
    {
        private static readonly Regex CardRegex = new(@"^Card[\s]+(?<cardNumber>[\d]+):", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public ScratchCard(string cardInfo) {
            var match = CardRegex.Match(cardInfo);
            if (match.Success && int.TryParse(match.Groups["cardNumber"].Value, out var gameNumber))
            {
                GameNumber = gameNumber;
                var cardNumberInfo = cardInfo.Substring(match.Index + match.Length);
                var cardNumberInfoParts = cardNumberInfo.Split("|");
                if (cardNumberInfoParts.Length > 1)
                {
                    var winningNums = cardNumberInfoParts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    WinningNumbers = winningNums.Select(int.Parse);
                    var playingNums = cardNumberInfoParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    PlayingNumbers = playingNums.Select(int.Parse);
                }
            }
            else
            {
                throw new Exception("Failed to parse card");
            }
        }

        public int GetPoints()
        {
            var points = 0;
            foreach( var winningNumber in WinningNumbers ) {
                if (PlayingNumbers.Contains(winningNumber))
                {
                    if (points == 0) { points = 1; }
                    else { points *= 2; }
                }
            }
            return points;
        }

        public int GetNumCardsToCopy() => WinningNumbers.Count( winningNumber => PlayingNumbers.Contains( winningNumber ) );

        public int GameNumber { get; set; }
        public int Copies { get; set; } = 1;
        public IEnumerable<int> WinningNumbers { get; set; }
        public IEnumerable<int> PlayingNumbers { get; set; }
    }
}
