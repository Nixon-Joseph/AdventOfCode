namespace AdventOfCode.Puzzles._2025.Day1
{
    public class PasswordSolution1 : PasswordBase
    {
        protected override object DoSolve()
        {
            var parts = ReadInputFromFile();
            var zeros = 0;
            var dial = new Dial();
            foreach (var part in parts)
            {
                var commandDir = part.ElementAt(0);
                var commandSteps = int.Parse(part.Substring(1));
                if (commandDir == 'L')
                {
                    dial.TurnLeft(commandSteps);
                }
                else if (commandDir == 'R')
                {
                    dial.TurnRight(commandSteps);
                }
                if (dial.Current == 0)
                {
                    zeros++;
                }
            }
            return zeros;
        }

        private class Dial
        {
            public int Current { get; set; } = 50;
            public int MaxNumber { get; set; } = 99;
            public int MinNumber { get; set; } = 0;

            public void TurnLeft(int steps)
            {
                Current -= steps % (MaxNumber + 1);
                if (Current < MinNumber)
                {
                    Current += MaxNumber + 1;
                }
            }

            public void TurnRight(int steps)
            {
                Current += steps % (MaxNumber + 1);
                if (Current > MaxNumber)
                {
                    Current -= MaxNumber + 1;
                }
            }
        }
    }
}
