namespace AdventOfCode.Puzzles._2023.Day16
{
    internal abstract class EnergizerBase : BaseSolution<char[,]>
    {
        internal override char[,] ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@".\Puzzles\2023\Day16\Input.txt");
            int rows = lines.Length;
            int cols = lines[0].Length;
            char[,] result = new char[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = lines[i][j];
                }
            }
            return result;
        }

        protected int GetNumEnergizedFromEntry(LightPath startPath)
        {
            var input = ReadInputFromFile();

            var energizedTiles = new string[input.GetLength(0), input.GetLength(1)];
            var lightPaths = new List<LightPath> { startPath };
            var sum = 0;

            while (lightPaths.Count > 0)
            {
                var pathsThisPass = lightPaths.Count;
                var pathsToAdd = new List<LightPath>();
                for (int pathIndex = pathsThisPass - 1; pathIndex >= 0; pathIndex--)
                {
                    var path = lightPaths[pathIndex];
                    if (path.X < 0 || path.X >= input.GetLength(1) || path.Y < 0 || path.Y >= input.GetLength(0))
                    {
                        lightPaths.RemoveAt(pathIndex);
                        continue; // reached the end of the grid
                    }
                    if (string.IsNullOrEmpty(energizedTiles[path.Y, path.X]) || !energizedTiles[path.Y, path.X].Contains(path.Direction))
                    {
                        if (string.IsNullOrEmpty(energizedTiles[path.Y, path.X]))
                        {
                            sum++;
                            energizedTiles[path.Y, path.X] = path.Direction.ToString();
                        }
                        else
                        {
                            energizedTiles[path.Y, path.X] += path.Direction;
                        }
                    }
                    else
                    {
                        lightPaths.RemoveAt(pathIndex);
                        continue; // been here before in this direction. Don't need to do it again.
                    }
                    var curChar = input[path.Y, path.X];

                    switch (curChar)
                    {
                        case '/':
                            path.Direction = path.Direction switch { 'E' => 'N', 'W' => 'S', 'N' => 'E', 'S' => 'W', _ => throw new NotImplementedException() };
                            break;
                        case '\\':
                            path.Direction = path.Direction switch { 'E' => 'S', 'W' => 'N', 'N' => 'W', 'S' => 'E', _ => throw new NotImplementedException() };
                            break;
                        case '|':
                            if (path.Direction == 'E' || path.Direction == 'W')
                            {
                                path.Direction = 'N';
                                pathsToAdd.Add(new(path.X, path.Y + 1, 'S'));
                            }
                            break;
                        case '-':
                            if (path.Direction == 'N' || path.Direction == 'S')
                            {
                                path.Direction = 'W';
                                pathsToAdd.Add(new(path.X + 1, path.Y, 'E'));
                            }
                            break;
                    }
                    path.Y = path.Direction switch { 'N' => path.Y - 1, 'S' => path.Y + 1, _ => path.Y };
                    path.X = path.Direction switch { 'E' => path.X + 1, 'W' => path.X - 1, _ => path.X };
                }
                lightPaths.AddRange(pathsToAdd);
            }

            //for (int y = 0; y < input.GetLength(0); y++)
            //{
            //    for (int x = 0; x < input.GetLength(1); x++)
            //    {
            //        if (!string.IsNullOrEmpty(energizedTiles[y, x]))
            //        {
            //            Console.Write('#');
            //        }
            //        else
            //        {
            //            Console.Write('.');
            //        }
            //    }
            //    Console.WriteLine();
            //}

            return sum;
        }

        public class LightPath
        {
            public LightPath(int x, int y, char dir)
            {
                X = x;
                Y = y;
                Direction = dir;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public char Direction { get; set; }
        }
    }
}
