using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day03
{
    internal class SchematicSolution1 : SchematicBase
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

            var sum = 0;
            foreach (var symbol in symbols)
            {
                var adjacentNumber = GetAdjacentNumber(symbol, numbers);
                if (adjacentNumber.HasValue)
                {
                    sum += adjacentNumber.Value;
                }
            }
            return sum;
        }

        private int? GetAdjacentNumber(Symbol symbol, List<List<Number>> numbers)
        {
            int foundNumber = 0;
            var rowStartCheck = Math.Max(symbol.Y - 1, 0);
            var rowEndCheck = Math.Min(symbol.Y + 1, numbers.Count);
            var curRowIndex = rowStartCheck;
            while (curRowIndex <= rowEndCheck)
            {
                var curRow = numbers.ElementAt(curRowIndex++);
                foundNumber += curRow.Sum(n => AdjacentValue(symbol.X, n.X, n.Length, n.Value));
            }

            return foundNumber;
        }
        // symbolX = 7, numX = 8, numLength = 3
        // ...788......
        // ..../..*963...
        private int AdjacentValue(int symbolX, int numX, int numLength, int value) => symbolX == numX || (symbolX > numX && numX + numLength - 1 >= symbolX - 1) || (symbolX < numX && symbolX + 1 == numX) ? value : 0;
    }

    internal class Number
    {
        public Number(int x, int length, int value)
        {
            X = x;
            Length = length;
            Value = value;
        }

        public int X { get; set; }
        public int Length { get; set; }
        public int Value { get; set; }
    }

    internal class Symbol
    {
        public Symbol(int x, int y, char value)
        {
            X = x;
            Y = y;
            Value = value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Value { get; set; }
    }
}
