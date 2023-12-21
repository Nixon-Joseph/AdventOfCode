using System.Diagnostics;

namespace AdventOfCode.Puzzles
{
    public abstract class BaseSolution<TFileRead> : ISolution
    {
        internal abstract TFileRead ReadInputFromFile();

        protected abstract object DoSolve();

        public object Solve()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine($"Solving {GetType().Name}...");
            var result = DoSolve();
            stopWatch.Stop();
            Console.WriteLine($"Solved {GetType().Name} in {stopWatch.ElapsedMilliseconds}ms");
            return result;
        }

        protected IEnumerable<string> ReadFileAsLines(string fileName) => File.ReadAllLines(fileName);

        protected char[,] ReadInputFileAsCharArray(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);
            int rows = lines.Length;
            int cols = lines[0].Length;
            var result = new char[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = lines[i][j];
                }
            }
            return result;
        }

        protected int[,] ReadInputFileAsIntArray(string inputFilePath)
        {
            var lines = File.ReadAllLines(inputFilePath);
            int rows = lines.Length;
            int cols = lines[0].Length;
            var result = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = int.Parse(lines[i][j].ToString());
                }
            }
            return result;
        }
    }
}
