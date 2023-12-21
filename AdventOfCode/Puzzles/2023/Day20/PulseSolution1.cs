namespace AdventOfCode.Puzzles._2023.Day20
{
    internal class PulseSolution1 : PulseBase
    {
        protected override object DoSolve()
        {
            var module = ReadInputFromFile();

            long highPulses = 0;
            long lowPulses = 0;

            Module.HighPulse += (sender, e) => highPulses++;
            Module.LowPulse += (sender, e) => lowPulses++;

            for (int i = 0; i < 1000; i++)
            {
                //Console.BackgroundColor = ConsoleColor.DarkBlue;
                //Console.WriteLine($"-------------------------- Pulse {i + 1} --------------------------");
                //Console.ResetColor();
                module.Press();
            }

            Console.WriteLine($"High Pulses: {highPulses}");
            Console.WriteLine($"Low Pulses: {lowPulses}");

            return highPulses * lowPulses;
        }
    }
}
