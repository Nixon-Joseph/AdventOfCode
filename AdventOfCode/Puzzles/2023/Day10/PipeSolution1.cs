namespace AdventOfCode.Puzzles._2023.Day10
{
    internal class PipeSolution1 : PipeBase
    {
        public override object Solve()
        {
            var maze = ReadInputFromFile();
            var (startX, startY) = FindStartingPoint(maze);
            var firstConnections = FindConnections(maze, startX, startY);
            (int x, int y)[] currentPositions = { firstConnections[0], firstConnections[1] };
            (int x, int y)[] lastPositions = { (startX, startY), (startX, startY) };
            var stepCount = 1;
            while (currentPositions[0].x != currentPositions[1].x || currentPositions[0].y != currentPositions[1].y)
            {
                var connections = currentPositions.Select(p => FindConnections(maze, p.x, p.y)).ToList();
                for (int i = 0; i < currentPositions.Length; i++)
                {
                    var connection = connections.ElementAt(i);
                    if (connection.First().x == lastPositions[i].x && connection.First().y == lastPositions[i].y)
                    {
                        lastPositions[i] = currentPositions[i];
                        currentPositions[i] = connection.Last();
                    }
                    else
                    {
                        lastPositions[i] = currentPositions[i];
                        currentPositions[i] = connection.First();
                    }
                }
                stepCount++;
            }   
            return stepCount;
        }
    }
}
