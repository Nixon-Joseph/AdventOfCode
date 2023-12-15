using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day03
{
    internal class SchematicSolution2 : SchematicBase
    {
        private static readonly Regex _numAndSymbolRegex = new(@"(?<symbol>[^\d.\s])|(?<number>[\d]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected override object DoSolve()
        {
            var lines = ReadInputFromFile();
            List<Symbol> symbols = new();
            List<List<Number>> numbers = new();
            var lineIndex = 0;
            // build collection of symbols and numbers, keeping track of their position.
            foreach (var line in lines)
            {
                var startOffset = 0;
                var numList = new List<Number>();
                while (startOffset < line.Length && _numAndSymbolRegex.Match(line, startOffset) is var match && match.Success)
                {
                    if (int.TryParse(match.Value, out var num))
                    {
                        numList.Add(new Number(match.Index, match.Length, num));
                    }
                    else
                    {
                        symbols.Add(new Symbol(match.Index, lineIndex, match.Value[0]));
                    }
                    startOffset = match.Index + match.Length;
                }
                numbers.Add(numList);
                lineIndex++;
            }

            var sum = symbols.Sum(s => GetAdjacentNumbersForGear(s, numbers));
            return sum;
        }

        private int GetAdjacentNumbersForGear(Symbol symbol, List<List<Number>> numbers)
        {
            var rowStartCheck = Math.Max(symbol.Y - 1, 0);
            var rowEndCheck = Math.Min(symbol.Y + 1, numbers.Count);
            var curRowIndex = rowStartCheck;
            var foundAdjacents = new List<Number>();
            while (curRowIndex <= rowEndCheck)
            {
                var curRow = numbers.ElementAt(curRowIndex++);
                foundAdjacents.AddRange(curRow.Where(n => IsAdjacent(symbol.X, n.X, n.Length)));
            }
            if (foundAdjacents.Count == 2)
            {
                return foundAdjacents[0].Value * foundAdjacents[1].Value;
            }
            return 0;
        }

        private bool IsAdjacent(int symbolX, int numX, int numLength) => symbolX == numX || (symbolX > numX && numX + numLength - 1 >= symbolX - 1) || (symbolX < numX && symbolX + 1 == numX);
    }
}
