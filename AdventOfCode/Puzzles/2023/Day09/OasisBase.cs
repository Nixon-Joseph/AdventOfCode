namespace AdventOfCode.Puzzles._2023.Day09
{
    internal abstract class OasisBase : BaseSolution<IEnumerable<List<int>>>
    {
        override internal IEnumerable<List<int>> ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@".\Puzzles\2023\Day09\Input.txt");
            var lists = new List<List<int>>();
            foreach (var line in lines)
            {
                var numbers = new List<int>();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    numbers.AddRange(line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
                }
                lists.Add(numbers);
            }
            return lists;
        }
    }
}
