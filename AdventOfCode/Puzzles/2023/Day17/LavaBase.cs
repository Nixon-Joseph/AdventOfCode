namespace AdventOfCode.Puzzles._2023.Day17
{
    internal abstract class LavaBase : BaseSolution<int[,]>
    {
        internal override int[,] ReadInputFromFile()
        {
            return ReadInputFileAsIntArray(@".\Puzzles\2023\Day17\Input.txt");
        }
    }
}
