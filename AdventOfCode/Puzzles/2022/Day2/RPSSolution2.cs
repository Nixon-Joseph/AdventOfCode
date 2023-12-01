namespace AdventOfCode.Puzzles.Day2
{
    internal class RPSSolution2 : ElfRPSBase<RPSStrategy>
    {
        public override object Solve()
        {
            var parsedInputs = ReadInputFromFile();
            var totalScore = 0;
            foreach (var round in parsedInputs)
            {
                totalScore += GetScore(round.opponent, GetStategicResponse(round.opponent, round.me));
            }
            return totalScore.ToString();
        }

        private RPS GetStategicResponse(RPS opponent, RPSStrategy me)
        {
            return me switch
            {
                RPSStrategy.Lose => opponent switch
                {
                    RPS.Rock => RPS.Scissors,
                    RPS.Paper => RPS.Rock,
                    RPS.Scissors => RPS.Paper,
                    _ => throw new ArgumentException("Invalid Input")
                },
                RPSStrategy.Tie => opponent,
                RPSStrategy.Win => opponent switch
                {
                    RPS.Rock => RPS.Paper,
                    RPS.Paper => RPS.Scissors,
                    RPS.Scissors => RPS.Rock,
                    _ => throw new ArgumentException("Invalid Input")
                },
                _ => throw new ArgumentException("Invalid input")
            };
        }

        internal override RPSStrategy ParseTInput(string input) => input.ToUpper() switch
        {
            "X" => RPSStrategy.Lose,
            "Y" => RPSStrategy.Tie,
            "Z" => RPSStrategy.Win,
            _ => throw new ArgumentException("Invalid input")
        };
    }

    internal enum RPSStrategy
    {
        Lose,
        Tie,
        Win
    }
}
