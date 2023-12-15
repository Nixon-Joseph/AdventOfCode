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
    }
}
