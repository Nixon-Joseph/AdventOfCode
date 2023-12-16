namespace AdventOfCode.Puzzles._2023.Day14
{
    internal class RockSolution2 : RockBase
    {
        protected override object DoSolve()
        {
            var lines = ReadInputFromFile();
            int cycles = 1000000000;
            var cycleIndexStart = 900;
            var cycleIndexEnd = 0;
            List<string> matchedCycle = new();
            for (int cycle = 0; cycle < cycles; cycle++)
            {
                for (int direction = 0; direction <= 3; direction++)
                {
                    // north
                    if (direction == 0)
                    {
                        for (int y = 0; y < lines.Count; y++)
                        {
                            for (int x = 0; x < lines[y].Length; x++)
                            {
                                if (lines[y][x] == 'O')
                                {
                                    int newY = y;
                                    while (newY > 0 && lines[newY - 1][x] != '#' && lines[newY - 1][x] != 'O')
                                    {
                                        newY--;
                                    }
                                    if (newY != y)
                                    {
                                        char[] line = lines[y].ToCharArray();
                                        line[x] = '.';
                                        lines[y] = new string(line);

                                        line = lines[newY].ToCharArray();
                                        line[x] = 'O';
                                        lines[newY] = new string(line);
                                    }
                                }
                            }
                        }
                    }
                    // west
                    else if (direction == 1)
                    {
                        for (int y = 0; y < lines.Count; y++)
                        {
                            for (int x = 0; x < lines[y].Length; x++)
                            {
                                if (lines[y][x] == 'O')
                                {
                                    int newX = x;
                                    while (newX > 0 && lines[y][newX - 1] != '#' && lines[y][newX - 1] != 'O')
                                    {
                                        newX--;
                                    }
                                    if (newX != x)
                                    {
                                        char[] line = lines[y].ToCharArray();
                                        line[x] = '.';
                                        lines[y] = new string(line);

                                        line = lines[y].ToCharArray();
                                        line[newX] = 'O';
                                        lines[y] = new string(line);
                                    }
                                }
                            }
                        }
                    }
                    // south
                    else if (direction == 2)
                    {
                        for (int y = lines.Count - 1; y >= 0; y--)
                        {
                            for (int x = 0; x < lines[y].Length; x++)
                            {
                                if (lines[y][x] == 'O')
                                {
                                    int newY = y;
                                    while (newY < lines.Count - 1 && lines[newY + 1][x] != '#' && lines[newY + 1][x] != 'O')
                                    {
                                        newY++;
                                    }
                                    if (newY != y)
                                    {
                                        char[] line = lines[y].ToCharArray();
                                        line[x] = '.';
                                        lines[y] = new string(line);

                                        line = lines[newY].ToCharArray();
                                        line[x] = 'O';
                                        lines[newY] = new string(line);
                                    }
                                }
                            }
                        }
                    }
                    // east
                    else if (direction == 3)
                    {
                        for (int y = 0; y < lines.Count; y++)
                        {
                            for (int x = lines[y].Length - 1; x >= 0; x--)
                            {
                                if (lines[y][x] == 'O')
                                {
                                    int newX = x;
                                    while (newX < lines[y].Length - 1 && lines[y][newX + 1] != '#' && lines[y][newX + 1] != 'O')
                                    {
                                        newX++;
                                    }
                                    if (newX != x)
                                    {
                                        char[] line = lines[y].ToCharArray();
                                        line[x] = '.';
                                        lines[y] = new string(line);

                                        line = lines[y].ToCharArray();
                                        line[newX] = 'O';
                                        lines[y] = new string(line);
                                    }
                                }
                            }
                        }
                    }
                }
                if (matchedCycle.Count == 0 && cycleIndexStart == cycle)
                {
                    matchedCycle = lines.Select(s => new string(s)).ToList();
                }
                else if (matchedCycle.Count > 0 && matchedCycle.SequenceEqual(lines))
                {
                    cycleIndexEnd = cycle;
                    var diff = cycleIndexEnd - cycleIndexStart;
                    while (cycle + diff < cycles)
                    {
                        cycle += diff;
                    }
                }
            }

            int sum = 0;
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == 'O')
                    {
                        sum += lines.Count - y;
                    }
                }
            }
            return sum;
        }

    }
}
