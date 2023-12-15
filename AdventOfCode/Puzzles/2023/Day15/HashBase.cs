namespace AdventOfCode.Puzzles._2023.Day15
{
    internal abstract class HashBase : BaseSolution<List<string>>
    {
        internal override List<string> ReadInputFromFile()
        {
            return File.ReadAllText("./Puzzles/2023/Day15/input.txt").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static int Hash(string input)
        {
            var value = 0;
            foreach (var character in input)
            {
                value += (int)character;
                value *= 17;
                value %= 256;
            }
            return value;
        }
    }
}
