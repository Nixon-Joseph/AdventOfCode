namespace AdventOfCode.Puzzles._2023.Day15
{
    internal class HashSolution1 : HashBase
    {
        protected override object DoSolve()
        {
            var input = ReadInputFromFile();
            var sum = 0;
            foreach (var item in input)
            {
                sum += Hash(item);
            }
            return sum;
        }
    }
}
