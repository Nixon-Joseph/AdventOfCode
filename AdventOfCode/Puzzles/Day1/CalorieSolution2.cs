namespace AdventOfCode.Puzzles.Day1
{
    internal class CalorieSolution2 : ElfCalorieBase
    {
        public override object Solve()
        {
            var parts = ReadInputFromFile();
            var totalCalsForElvs = parts.Select(x => x.Sum());
            return totalCalsForElvs.OrderByDescending(x => x).Take(3).Sum().ToString();
        }
    }
}
