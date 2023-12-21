namespace AdventOfCode.Puzzles._2023.Day17
{
    internal class LavaSolution1 : LavaBase
    {
        protected override object DoSolve()
        {
            var input = ReadInputFromFile();
            var path = new Path();
            var finalPosition = new DirectionalXYCoordForAStar { X = input.GetLength(1) - 1, Y = input.GetLength(0) - 1, Value = input[input.GetLength(0) - 1, input.GetLength(1) - 1] };
            var node = new DirectionalXYCoordForAStar { X = 0, Y = 0, Value = input[0, 0] };
            var openNodes = new List<DirectionalXYCoordForAStar>();
            var closedNodes = new List<DirectionalXYCoordForAStar>();

            var solver = new WeightedGridPathSolver(input);

            var result = solver.Solve(node, finalPosition);

            DrawMap(result, input);

            return result.Sum(r => (int)r.Value);


            //var latestPos = new DirectionalXYCoord { Direction = Direction.Right, X = 0, Y = 0 };
            //latestPos.Value = input[latestPos.Y, latestPos.X];
            //path.Add(latestPos);
            //var iterations = 0;
            //while (latestPos.DistanceTo(finalPosition) > 1d)
            //{
            //    var paths = BuildPaths(input, path);
            //    if (paths.Any(p => p.Coords.Last().DistanceTo(finalPosition) == 0))
            //    {
            //        var bestPath = paths.OrderBy(p => p.HeatLoss).First(p => p.Coords.Last().DistanceTo(finalPosition) == 0);
            //        foreach (var coord in bestPath.Coords)
            //        {
            //            path.Add(coord);
            //        }
            //        latestPos = path.Coords.Last();
            //    }
            //    else
            //    {
            //        var sortedPaths = paths
            //            .OrderBy(p => p.Coords.Last().DistanceTo(finalPosition) - p.AverageHeatLoss)
            //            .ThenBy(p => p.HeatLoss)
            //            .ToList();
            //        var bestPath = sortedPaths.ElementAt(0);
            //        foreach (var coord in bestPath.Coords)
            //        {
            //            path.Add(coord);
            //        }
            //        latestPos = path.Coords.Last();
            //        //do
            //        //{
            //        //    latestPos = bestPath.Coords[0];
            //        //} while (!path.Add(latestPos));
            //        if (iterations % Math.Min(50, (int)latestPos.DistanceTo(finalPosition)) == 0)
            //        {
            //            DrawMap(path, input);
            //        }
            //        iterations++;
            //    }
            //}
            //DrawMap(path, input);

            //return path.HeatLoss;
        }

        private void DrawHeightMap(int[,] input)
        {
            Console.Clear();
            for (int y = 0; y < input.GetLength(0); y++)
            {
                for (int x = 0; x < input.GetLength(1); x++)
                {
                    Console.BackgroundColor = input[y, x] switch
                    {
                        1 => ConsoleColor.White,
                        2 => ConsoleColor.Green,
                        3 => ConsoleColor.DarkGreen,
                        4 => ConsoleColor.Cyan,
                        5 => ConsoleColor.DarkCyan,
                        6 => ConsoleColor.Blue,
                        7 => ConsoleColor.DarkBlue,
                        8 => ConsoleColor.Red,
                        9 => ConsoleColor.DarkRed,
                        _ => ConsoleColor.Black
                    };
                    Console.Write(input[y, x]);
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }

        private void DrawMap(List<DirectionalXYCoord> path, int[,] input)
        {
            Console.Clear();
            var pathMap = path.ToDictionary(p => (p.X, p.Y));
            for (int y = 0; y < input.GetLength(0); y++)
            {
                for (int x = 0; x < input.GetLength(1); x++)
                {
                    if (pathMap.ContainsKey((x, y)))
                    {
                        // highlight next console output char
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.Write(input[y, x]);
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(input[y, x]);
                    }
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }
        }

        //private List<Path> BuildPaths(int[,] input, Path curPath, List<Path>? paths = null, int curDepth = 0)
        //{
        //    paths ??= new List<Path>();
        //    if (curDepth < searchDepth)
        //    {
        //        var coord = curPath.Coords.Last();
        //        var lastDirections = curPath.Coords.TakeLast(3).Select(c => c.Direction).ToList();
        //        var availableDirs = curPath.Coords.Last().Direction switch
        //        {
        //            Direction.Up => new List<Direction> { Direction.Up, Direction.Left, Direction.Right },
        //            Direction.Down => new List<Direction> { Direction.Down, Direction.Left, Direction.Right },
        //            Direction.Left => new List<Direction> { Direction.Up, Direction.Down, Direction.Left },
        //            Direction.Right => new List<Direction> { Direction.Up, Direction.Down, Direction.Right },
        //            _ => throw new Exception("Invalid direction")
        //        };

        //        for (int i = 0; i < availableDirs.Count; i++)
        //        {
        //            var dir = availableDirs[i];
        //            // can't go the same direction more than 3 times in a row
        //            if (lastDirections.Count() == 3 && lastDirections.ElementAt(0) == dir && lastDirections.ElementAt(1) == dir)
        //            {
        //                continue;
        //            }
        //            var newPos = dir switch
        //            {
        //                Direction.Up => new DirectionalXYCoord { Direction = dir, X = coord.X, Y = coord.Y - 1 },
        //                Direction.Down => new DirectionalXYCoord { Direction = dir, X = coord.X, Y = coord.Y + 1 },
        //                Direction.Left => new DirectionalXYCoord { Direction = dir, X = coord.X - 1, Y = coord.Y },
        //                Direction.Right => new DirectionalXYCoord { Direction = dir, X = coord.X + 1, Y = coord.Y },
        //                _ => throw new Exception("Invalid direction")
        //            };
        //            // out of bounds
        //            if (newPos.X < 0 || newPos.Y < 0 || newPos.X >= input.GetLength(1) || newPos.Y >= input.GetLength(0))
        //            {
        //                continue;
        //            }
        //            newPos.Value = input[newPos.Y, newPos.X];
        //            var dupePath = curPath.Duplicate();
        //            if (!dupePath.Add(newPos))
        //            {
        //                // skip paths that go back onto existing path
        //                continue;
        //            }
        //            if (newPos.X == input.GetLength(1) - 1 && newPos.Y == input.GetLength(0))
        //            {
        //                paths.Add(dupePath.TakeLast(curDepth));
        //                continue;
        //            }
        //            var _ = BuildPaths(input, dupePath, paths, curDepth + 1);
        //        }
        //    }
        //    else
        //    {
        //        paths.Add(curPath.TakeLast(curDepth));
        //    }

        //    return paths;
        //}

        private class DirectionalXYCoordForAStar : DirectionalXYCoord
        {
            public DirectionalXYCoordForAStar Parent { get; set; }
        }

        private class Path
        {
            private Dictionary<(int x, int y), bool> CoordDict { get; set; }
            public List<DirectionalXYCoord> Coords { get; set; }
            public int HeatLoss { get; set; }
            public bool Add(DirectionalXYCoord coord)
            {
                if (!CoordDict.ContainsKey((coord.X, coord.Y)))
                {
                    Coords.Add(coord);
                    CoordDict.Add((coord.X, coord.Y), true);
                    HeatLoss += int.Parse(coord.Value.ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }

            internal Path Duplicate()
            {
                return new Path
                {
                    Coords = new List<DirectionalXYCoord>(Coords),
                    HeatLoss = HeatLoss,
                    CoordDict = new Dictionary<(int x, int y), bool>(CoordDict)
                };
            }

            internal Path TakeLast(int numToTake)
            {
                var newCoords = Coords.TakeLast(numToTake).ToList();
                return new Path
                {
                    Coords = new List<DirectionalXYCoord>(newCoords),
                    HeatLoss = newCoords.Sum(c => int.Parse(c.Value.ToString())),
                    CoordDict = newCoords.ToDictionary(c => (c.X, c.Y), c => true)
                };
            }

            public Path()
            {
                Coords = new List<DirectionalXYCoord>();
                CoordDict = new Dictionary<(int x, int y), bool>();
            }

            public int AverageHeatLoss { get { return HeatLoss / Coords.Count; } }
        }
    }
}
