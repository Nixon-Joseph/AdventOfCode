using System.Text;

namespace AdventOfCode.Puzzles._2023.Day10
{
    public abstract class PipeBase : BaseSolution<char[,]>
    {
        internal override char[,] ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@".\Puzzles\2023\Day10\Input.txt");
            var maze = new char[lines.Length, lines[0].Length];
            Console.OutputEncoding = Encoding.UTF8;

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Replace('|', '║').Replace('-', '═').Replace('L', '╚').Replace('J', '╝').Replace('7', '╗').Replace('F', '╔');
                //Console.WriteLine(line);
                for (var j = 0; j < line.Length; j++)
                {
                    maze[j, i] = line[j];
                }
            }

            return maze;
        }

        protected static (int x, int y) FindStartingPoint(char[,] maze)
        {
            var startX = -1;
            var startY = -1;
            for (var i = 0; i < maze.GetLength(0); i++)
            {
                for (var j = 0; j < maze.GetLength(1); j++)
                {
                    if (maze[i, j] == 'S')
                    {
                        return (i, j);
                    }
                }
            }
            return (startX, startY);
        }

        protected static (int x, int y)[] FindConnections(char[,] maze, int x, int y)
        {
            var connections = new List<(int, int)>();
            switch (maze[x, y])
            {
                case '╚':
                    connections.Add((x, y - 1));
                    connections.Add((x + 1, y));
                    break;
                case '╝':
                    connections.Add((x, y - 1));
                    connections.Add((x - 1, y));
                    break;
                case '╗':
                    connections.Add((x - 1, y));
                    connections.Add((x, y + 1));
                    break;
                case '╔':
                    connections.Add((x + 1, y));
                    connections.Add((x, y + 1));
                    break;
                case '║':
                    connections.Add((x, y - 1));
                    connections.Add((x, y + 1));
                    break;
                case '═':
                    connections.Add((x - 1, y));
                    connections.Add((x + 1, y));
                    break;
                case 'S': // I'm cheating
                    connections.Add((x, y - 1));
                    connections.Add((x, y + 1));
                    break;
                default:
                    throw new Exception($"Unknown connector: {maze[x, y]}");
            }
            return connections.ToArray();
        }
    }
}
