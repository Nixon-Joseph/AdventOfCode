namespace AdventOfCode.Puzzles._2025.Day1
{
    public abstract class PasswordBase : BaseSolution<IEnumerable<string>>
    {
        internal override IEnumerable<string> ReadInputFromFile()
        {
            var parts = new List<string>();
            var lines = File.ReadLines("./Puzzles/2025/Day1/Input.txt");
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    parts.Add(line);
                }
            }
            return parts;
        }
    }
}
