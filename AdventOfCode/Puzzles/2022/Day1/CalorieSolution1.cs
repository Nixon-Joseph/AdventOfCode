namespace AdventOfCode.Puzzles.Day1
{
    public class CalorieSolution1 : ElfCalorieBase
    {
        public override object Solve()
        {
            var parts = ReadInputFromFile();
            return parts.Max(x => x.Sum()).ToString();
        }
    }
}
