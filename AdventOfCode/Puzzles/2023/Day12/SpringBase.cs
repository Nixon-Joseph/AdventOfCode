namespace AdventOfCode.Puzzles._2023.Day12
{
    internal abstract class SpringBase : BaseSolution<string[]>
    {
        internal override string[] ReadInputFromFile()
        {
            return File.ReadAllLines(@".\Puzzles\2023\Day12\Input.txt");
        }
    }
}
