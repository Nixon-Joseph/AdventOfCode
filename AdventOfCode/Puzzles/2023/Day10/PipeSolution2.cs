namespace AdventOfCode.Puzzles._2023.Day10
{
    internal class PipeSolution2 : PipeBase
    {
        private enum PositionType
        {
            Unchecked,
            Inside,
            Outside,
            Loop
        }

        public override object Solve()
        {
            var maze = ReadInputFromFile();
            var (startX, startY) = FindStartingPoint(maze);
            var firstConnections = FindConnections(maze, startX, startY);
            (int x, int y)[] currentPositions = { firstConnections[0], firstConnections[1] };
            (int x, int y)[] lastPositions = { (startX, startY), (startX, startY) };
            var loopParts = new PositionType[maze.GetLength(0), maze.GetLength(1)];
            loopParts[0, 0] = PositionType.Outside;
            loopParts[startX, startY] = PositionType.Loop;
            loopParts[firstConnections[0].x, firstConnections[0].y] = PositionType.Loop;
            loopParts[firstConnections[1].x, firstConnections[1].y] = PositionType.Loop;
            var insideLoopCount = 0;
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
                    loopParts[currentPositions[i].x, currentPositions[i].y] = PositionType.Loop;
                }
            }
            var x = 0;
            var y = 0;
            while (y < maze.GetLength(0))
            {
                x = 0;
                while (x < maze.GetLength(1))
                {
                    loopParts[x, y] = GetPositionType(loopParts, maze, x, y);
                    x++;
                }
                y++;
            }
            y = maze.GetLength(0) - 1;
            while (y > 0)
            {
                x = maze.GetLength(1) - 1;
                while (x > 0)
                {
                    loopParts[x, y] = GetPositionType(loopParts, maze, x, y);
                    x--;
                }
                y--;
            }
            // follow the path around, we know the right side is the 'outside' at the start.
            // mark all the adjacent ? as inside/outside depending on how they sit in relation to the known outside
            (int x, int y) lastPos = (startX, startY);
            var currentPos = firstConnections[0];
            var outside = "R"; // cheating again
            while (currentPos.x != startX || currentPos.y != startY) // loop back to start all the way around.
            {
                var nextPos = FindConnections(maze, currentPos.x, currentPos.y).First(p => p.x != lastPos.x || p.y != lastPos.y);
                if (loopParts[currentPos.x - 1, currentPos.y] == PositionType.Unchecked) // left
                {
                    loopParts[currentPos.x - 1, currentPos.y] = outside.Contains('L') ? PositionType.Outside : PositionType.Inside;
                }
                if (loopParts[currentPos.x + 1, currentPos.y] == PositionType.Unchecked) // right
                {
                    loopParts[currentPos.x + 1, currentPos.y] = outside.Contains('R') ? PositionType.Outside : PositionType.Inside;
                }
                if (loopParts[currentPos.x, currentPos.y - 1] == PositionType.Unchecked) // up
                {
                    loopParts[currentPos.x, currentPos.y - 1] = outside.Contains('U') ? PositionType.Outside : PositionType.Inside;
                }
                if (loopParts[currentPos.x, currentPos.y + 1] == PositionType.Unchecked) // down
                {
                    loopParts[currentPos.x, currentPos.y + 1] = outside.Contains('D') ? PositionType.Outside : PositionType.Inside;
                }
                lastPos = currentPos;
                currentPos = nextPos;
                var directionOfTravel = "?";
                if (currentPos.y < lastPos.y) { directionOfTravel = "U"; }
                else if (currentPos.y > lastPos.y) { directionOfTravel = "D"; }
                else if (currentPos.x < lastPos.x) { directionOfTravel = "L"; }
                else if (currentPos.x > lastPos.x) { directionOfTravel = "R"; }
                outside = GetOutsideDirection(maze, currentPos, outside, directionOfTravel);
            }
            // finish out remaining unchecked positions
            for (y = 0; y < maze.GetLength(0); y++)
            {
                for (x = 0; x < maze.GetLength(1); x++)
                {
                    if (loopParts[x, y] == PositionType.Unchecked)
                    {
                        if (loopParts[x - 1, y] == PositionType.Inside || loopParts[x + 1, y] == PositionType.Inside || loopParts[x, y - 1] == PositionType.Inside || loopParts[x, y + 1] == PositionType.Inside)
                        {
                            loopParts[x, y] = PositionType.Inside;
                            insideLoopCount++;
                        }
                    }
                    else if (loopParts[x, y] == PositionType.Inside)
                    {
                        insideLoopCount++;
                    }
                }
            }
            for (y = 0; y < maze.GetLength(0); y++)
            {
                for (x = 0; x < maze.GetLength(1); x++)
                {
                    switch (loopParts[x, y])
                    {
                        case PositionType.Inside:
                            if (maze[x, y] == '.')
                            {
                                Console.Write('I');
                            }
                            else
                            {
                                Console.Write(' ');
                            }
                            break;
                        case PositionType.Outside:
                            Console.Write(' ');
                            break;
                        case PositionType.Unchecked:
                            if (maze[x, y] == '.')
                            {
                                Console.Write('?');
                            }
                            else
                            {
                                Console.Write(' ');
                            }
                            break;
                        case PositionType.Loop:
                            Console.Write(maze[x, y]);
                            break;
                    }
                }
                Console.WriteLine();
            }
            return insideLoopCount;
        }

        private static string GetOutsideDirection(char[,] maze, (int x, int y) currentPos, string outside, string directionOfTravel)
        {
            switch (maze[currentPos.x, currentPos.y])
            {
                case '╔':
                    if (directionOfTravel == "U") { return outside.Contains('L') ? "LU" : "D"; }
                    if (directionOfTravel == "L") { return outside.Contains('U') ? "LU" : "R"; }
                    break;
                case '╗':
                    if (directionOfTravel == "U") { return outside.Contains('R') ? "RU" : "D"; }
                    if (directionOfTravel == "R") { return outside.Contains('U') ? "RU" : "L"; }
                    break;
                case '╚':
                    if (directionOfTravel == "D") { return outside.Contains('L') ? "LD" : "U"; }
                    if (directionOfTravel == "L") { return outside.Contains('D') ? "LD" : "R"; }
                    break;
                case '╝':
                    if (directionOfTravel == "D") { return outside.Contains('R') ? "RD" : "U"; }
                    if (directionOfTravel == "R") { return outside.Contains('D') ? "RD" : "L"; }
                    break;
                case '═':
                    if (directionOfTravel == "L") { return outside.Contains('U') ? "U" : "D"; }
                    if (directionOfTravel == "R") { return outside.Contains('U') ? "U" : "D"; }
                    break;
                case '║':
                    if (directionOfTravel == "U") { return outside.Contains('L') ? "L" : "R"; }
                    if (directionOfTravel == "D") { return outside.Contains('L') ? "L" : "R"; }
                    break;
            }
            return outside;
        }

        private PositionType GetPositionType(PositionType[,] loopParts, char[,] maze, int x, int y)
        {
            if (loopParts[x, y] == PositionType.Unchecked)
            {
                if (
                    (x > 0 && loopParts[x - 1, y] == PositionType.Outside) ||
                    (y > 0 && loopParts[x, y - 1] == PositionType.Outside) ||
                    (x < maze.GetLength(1) - 1 && loopParts[x + 1, y] == PositionType.Outside) ||
                    (y < maze.GetLength(0) - 1 && loopParts[x, y + 1] == PositionType.Outside)
                )
                {
                    return PositionType.Outside;
                }
                return PositionType.Unchecked;
            }
            else
            {
                return loopParts[x, y];
            }
        }
    }
}
