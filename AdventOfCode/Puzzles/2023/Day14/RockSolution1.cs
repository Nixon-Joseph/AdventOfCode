namespace AdventOfCode.Puzzles._2023.Day14
{
    internal class RockSolution1 : RockBase
    {
        protected override object DoSolve()
        {
            var lines = ReadInputFromFile();
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
            foreach (var line in lines)
            {
                Console.WriteLine(line);
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
