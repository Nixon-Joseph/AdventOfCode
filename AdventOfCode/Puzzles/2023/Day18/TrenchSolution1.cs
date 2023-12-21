namespace AdventOfCode.Puzzles._2023.Day18
{
    internal class TrenchSolution1 : TrenchBase
    {
        protected override object DoSolve()
        {
            var instructions = ReadInputFromFile();
            var area = 0;
            var map = new List<List<char>>
            {
                new() { ' ' }
            };
            var curPos = new XYCoord(0, 0);
            foreach (var instruction in instructions)
            {
                for (var i = 0; i < instruction.Distance; i++)
                {
                    switch (instruction.Direction)
                    {
                        case Direction.Up:
                            if (curPos.Y == 0)
                            {
                                map.Insert(0, Enumerable.Repeat(' ', map[0].Count).ToList());
                                curPos.Y++;
                            }
                            curPos.Y--;
                            break;
                        case Direction.Down:
                            if (curPos.Y >= map.Count - 1)
                            {
                                map.Add(Enumerable.Repeat(' ', map[0].Count).ToList());
                            }
                            curPos.Y++;
                            break;
                        case Direction.Left:
                            if (curPos.X == 0)
                            {
                                for (var x = 0; x < map.Count; x++)
                                {
                                    map[x].Insert(0, ' ');
                                }
                                curPos.X++;
                            }
                            curPos.X--;
                            break;
                        case Direction.Right:
                            if (curPos.X >= map[0].Count - 1)
                            {
                                for (var x = 0; x < map.Count; x++)
                                {
                                    map[x].Add(' ');
                                }
                            }
                            curPos.X++;
                            break;
                    }
                    map[curPos.Y][curPos.X] = '#';
                }
            }
            //for (var y = 0; y < map.Count; y++)
            //{
            //    var lookingForFirst = true;
            //    for (var x = 0; x < map[0].Count; x++)
            //    {
            //        if (map[y][x] == '#')
            //        {
            //            lookingForFirst = !lookingForFirst;
            //            area++;
            //        }
            //        if (!lookingForFirst)
            //        {
            //            area++;
            //        }
            //        //Console.Write(map[y][x].Value);
            //    }
            //    //Console.WriteLine();
            //}
            area = CalculateArea(map);
            return area;
        }

        /*
        example input
        #######
        #     #
        ###   #
          #   #
          #   #
        ### ###
        #   #  
        ##  ###
         #    #
         ######
        area should be 62
         */

        public static int CalculateArea(List<List<char>> grid)
        {
            if (grid == null || grid.Count == 0 || grid[0].Count == 0)
            {
                return 0; // No shape in an empty grid
            }

            int numRows = grid.Count;
            int numCols = grid[0].Count;
            int totalArea = 0;

            bool[][] visited = new bool[numRows][];
            for (int i = 0; i < numRows; i++)
            {
                visited[i] = new bool[numCols];
            }

            // Counting the boundary area
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    if (grid[row][col] == '#' && !visited[row][col])
                    {
                        totalArea += FloodFill(grid, visited, row, col);
                    }
                }
            }

            // Counting the internal area
            for (int row = 0; row < numRows; row++)
            {
                int firstHash = -1, lastHash = -1;
                for (int col = 0; col < numCols; col++)
                {
                    if (grid[row][col] == '#')
                    {
                        if (firstHash == -1)
                            firstHash = col;
                        lastHash = col;
                    }
                }
                if (firstHash != -1 && lastHash != -1)
                {
                    totalArea += (lastHash - firstHash + 1);
                }
            }

            return totalArea;
        }


        private static int FloodFill(List<List<char>> grid, bool[][] visited, int row, int col)
        {
            int numRows = grid.Count;
            int numCols = grid[0].Count;

            if (row < 0 || row >= numRows || col < 0 || col >= numCols || visited[row][col] || grid[row][col] == ' ')
            {
                return 0;
            }

            visited[row][col] = true;
            int area = 1;

            area += FloodFill(grid, visited, row + 1, col);
            area += FloodFill(grid, visited, row - 1, col);
            area += FloodFill(grid, visited, row, col + 1);
            area += FloodFill(grid, visited, row, col - 1);

            return area;
        }
    }

    internal class TrenchNode
    {
        public TrenchNode(char value, string? colorHext)
        {
            Value = value;
            ColorHex = colorHext;
        }

        public char Value { get; set; }
        public string? ColorHex { get; set; }
    }
}
