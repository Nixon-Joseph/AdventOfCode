namespace AdventOfCode.Puzzles.Day1
{
    public class CalorieSolution1 : ElfCalorieBase
    {
        protected override object DoSolve()
        {
            var parts = ReadInputFromFile();
            return parts.Max(x => x.Sum()).ToString();
        }
    }
}
