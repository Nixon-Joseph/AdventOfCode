namespace AdventOfCode.Puzzles._2023
{
    internal abstract class CalibrationBase : BaseSolution<IEnumerable<string>>
    {
        internal override IEnumerable<string> ReadInputFromFile()
        {
            var lines = File.ReadLines("./Puzzles/2023/Day01/Input.txt");
            return lines;
        }
    }
}
