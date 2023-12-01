namespace AdventOfCode.Puzzles._2023.Day01
{
    internal class CalibrationSolution1 : CalibrationBase
    {
        public override object Solve()
        {
            var input = ReadInputFromFile();
            // find the first and last digit in each input line, and append them together.
            var result = 0;
            foreach (var line in input)
            {
                var first = '0';
                var last = '0';
                bool lookingForFirst = true;
                foreach( var ch in line ) {
                    if (int.TryParse(ch.ToString(), out var digit))
                    {
                        if (lookingForFirst)
                        {
                            first = digit.ToString()[0];
                            lookingForFirst = false;
                        }
                        last = digit.ToString()[0];
                    }
                }
                if (int.TryParse($"{first}{last}", out var calibrationValue))
                {
                    result += calibrationValue;
                }
            }
            return result;
        }
    }
}
