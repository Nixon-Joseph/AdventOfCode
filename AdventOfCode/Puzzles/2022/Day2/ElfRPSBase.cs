namespace AdventOfCode.Puzzles.Day2
{
    internal abstract class ElfRPSBase<T> : BaseSolution<IEnumerable<(RPS opponent, T me)>>
    {
        internal override IEnumerable<(RPS opponent, T me)> ReadInputFromFile()
        {
            var parts = new List<(RPS opponent, T me)>();
            var lines = File.ReadLines("./Puzzles/Day2/Input.txt");
            foreach (var line in lines)
            {
                var split = line.Split(' ');
                parts.Add((ParseInput(split[0]), ParseTInput(split[1])));
            }
            return parts;
        }

        internal RPS ParseInput(string input) => input.ToUpper() switch
        {
            "A" => RPS.Rock,
            "X" => RPS.Rock,
            "B" => RPS.Paper,
            "Y" => RPS.Paper,
            "C" => RPS.Scissors,
            "Z" => RPS.Scissors,
            _ => throw new ArgumentException("Invalid input")
        };

        internal int GetScore(RPS opponent, RPS me)
        {
            var score = (int)me;
            if (opponent == me)
            {
                score += 3;
            }
            else
            {
                if (
                    opponent == RPS.Rock && me == RPS.Paper ||
                    opponent == RPS.Paper && me == RPS.Scissors ||
                    opponent == RPS.Scissors && me == RPS.Rock
                )
                    score += 6;
                // else lose - no score
            }
            return score;
        }

        internal abstract T ParseTInput(string input);
    }

    internal enum RPS
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }
}
