namespace AdventOfCode.Puzzles._2023.Day04
{
    internal class ScratchSolution1 : ScratchBase
    {
        protected override object DoSolve()
        {
            var cards = ReadInputFromFile();
            return cards.Sum(c => c.GetPoints());
        }
    }
}
