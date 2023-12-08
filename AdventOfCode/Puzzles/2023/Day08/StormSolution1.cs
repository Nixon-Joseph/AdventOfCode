using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023.Day08
{
    internal class StormSolution1 : StormBase
    {
        public override object Solve()
        {
            var (directions, nodeDict) = ReadInputFromFile();
            Node node = nodeDict["AAA"];
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var numSteps = 0;
            var selectedNode = node;

            while (selectedNode.Name != "ZZZ")
            {
                foreach (var direction in directions)
                {
                    selectedNode = selectedNode[direction];
                    numSteps++;
                }
            }

            stopWatch.Stop();
            Console.WriteLine($"Solved in {stopWatch.ElapsedMilliseconds}ms");
            return numSteps;
        }
    }
}
