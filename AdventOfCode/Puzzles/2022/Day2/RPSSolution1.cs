namespace AdventOfCode.Puzzles.Day2
{
    internal class RPSSolution1 : ElfRPSBase<RPS>
    {
        protected override object DoSolve()
        {
            var parsedInputs = ReadInputFromFile();
            var totalScore = 0;
            foreach (var round in parsedInputs)
            {
                totalScore += GetScore(round.opponent, round.me);
            }
            return totalScore.ToString();
        }

        internal override RPS ParseTInput(string input) => ParseInput(input);
    }
}
