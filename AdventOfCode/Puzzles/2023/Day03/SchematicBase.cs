namespace AdventOfCode.Puzzles._2023.Day03
{
    internal abstract class SchematicBase : BaseSolution<IEnumerable<string>>
    {
        internal override IEnumerable<string> ReadInputFromFile()
        {
            {
                var lines = File.ReadLines("./Puzzles/2023/Day03/Input.txt");
                return lines;
            }
        }
    }
}
