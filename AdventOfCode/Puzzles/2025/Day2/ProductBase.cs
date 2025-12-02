namespace AdventOfCode.Puzzles._2025.Day2
{
    public abstract class ProductBase : BaseSolution<IEnumerable<(ulong low, ulong high)>>
    {
        internal override IEnumerable<(ulong low, ulong high)> ReadInputFromFile()
        {
            var parts = new List<(ulong low, ulong high)>();
            var input = File.ReadAllText("./Puzzles/2025/Day2/Input.txt");
            var groups = input.Split(",");
            foreach (var line in groups)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    var bounds = line.Split("-");
                    var low = ulong.Parse(bounds[0]);
                    var high = ulong.Parse(bounds[1]);
                    parts.Add((low, high));
                }
            }
            return parts;
        }
    }
}
