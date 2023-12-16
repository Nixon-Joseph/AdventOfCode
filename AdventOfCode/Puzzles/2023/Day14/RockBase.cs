namespace AdventOfCode.Puzzles._2023.Day14
{
    internal abstract class RockBase : BaseSolution<List<string>>
    {
        internal override List<string> ReadInputFromFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Puzzles", "2023", "Day14", "Input.txt");
            return File.ReadAllLines(path).ToList();
        }
    }
}