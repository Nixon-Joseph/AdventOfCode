namespace AdventOfCode.Puzzles.Day1
{
    public abstract class ElfCalorieBase : BaseSolution<IEnumerable<IEnumerable<int>>>
    {
        internal override IEnumerable<IEnumerable<int>> ReadInputFromFile()
        {
            var parts = new List<IEnumerable<int>>();
            var lines = File.ReadLines("./Puzzles/Day1/Input.txt");
            List<int> partList = null;
            foreach (var line in lines)
            {
                partList ??= new List<int>();
                if (int.TryParse(line, out var parsedInt))
                {
                    partList.Add(parsedInt);
                }
                else
                {
                    parts.Add(partList.ToArray());
                    partList = null;
                }
            }
            return parts;
        }
    }
}
