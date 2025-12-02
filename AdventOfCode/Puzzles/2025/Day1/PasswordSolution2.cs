namespace AdventOfCode.Puzzles._2025.Day1
{
    internal class PasswordSolution2 : PasswordBase
    {
        protected override object DoSolve()
        {
            var parts = ReadInputFromFile();
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
            }
            return dial.ZeroTickCounter;
        }

        private class Dial
        {
            public int Current { get; set; } = 50;
            public int MaxNumber { get; set; } = 99;
            public int MinNumber { get; set; } = 0;

            public int ZeroTickCounter { get; private set; } = 0;

            public void TurnLeft(int steps)
            {
                var remainderSteps = steps % (MaxNumber + 1);
                var loops = Math.Floor((steps - remainderSteps) / (double)(MaxNumber + 1));
                ZeroTickCounter += (int)loops;
                var oldCurrent = Current;
                Current -= remainderSteps;
                if (Current < MinNumber)
                {
                    if (oldCurrent > MinNumber)
                    {
                        ZeroTickCounter++;
                    }
                    Current += MaxNumber + 1;
                }
                else if (Current == MinNumber)
                {
                    ZeroTickCounter++;
                }
            }

            public void TurnRight(int steps)
            {
                var remainderSteps = steps % (MaxNumber + 1);
                var loops = Math.Floor((steps - remainderSteps) / (double)(MaxNumber + 1));
                ZeroTickCounter += (int)loops;
                Current += remainderSteps;
                if (Current > MaxNumber)
                {
                    ZeroTickCounter++;
                    Current -= MaxNumber + 1;
                }
                else if (Current == MinNumber)
                {
                    ZeroTickCounter++;
                }
            }
        }
    }
}
