namespace AdventOfCode.Puzzles.Day6
{
    public abstract class CommunicationSolutionBase : BaseSolution<string>
    {
        protected readonly int _charsToCheck;
        public CommunicationSolutionBase(int charsToCheck)
        {
            _charsToCheck = charsToCheck;
        }
        internal override string ReadInputFromFile() => File.ReadAllText("Puzzles/Day6/input.txt");
    }
}
