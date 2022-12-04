namespace AdventOfCode.Puzzles
{
    public abstract class BaseSolution<TFileRead> : ISolution
    {
        internal abstract TFileRead ReadInputFromFile();

        public abstract object Solve();
    }
}
