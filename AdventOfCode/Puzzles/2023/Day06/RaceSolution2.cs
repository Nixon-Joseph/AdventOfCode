using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023.Day06
{
    internal class RaceSolution2 : BaseSolution<Race>
    {
        internal override Race ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@"./Puzzles/2023/Day06/Input.txt");
            var time = long.Parse(string.Join("", lines[0][6..].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
            var distance = long.Parse(string.Join("", lines[1][10..].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
            return new Race { Time = time, Distance = distance };
        }

        // Optimization got it to 1ms solve time
        public override object Solve()
        {
            var timer = new Stopwatch();
            timer.Start();
            var race = ReadInputFromFile();

            var timeChargeStart = 1;
            var step = 10000;
            while (timeChargeStart < race.Time)
            {
                if (timeChargeStart * (race.Time - timeChargeStart) > race.Distance)
                { 
                    if (step == 1) { break; }
                    else
                    {
                        timeChargeStart -= step;
                        step /= 10;
                    }
                }
                timeChargeStart += step;
            }
            var timeChargeEnd = race.Time;
            step = 10000;
            while (timeChargeEnd > timeChargeStart)
            {
                if (timeChargeEnd * (race.Time - timeChargeEnd) > race.Distance)
                {
                    if (step == 1) { break; }
                    else
                    {
                        timeChargeEnd += step;
                        step /= 10;
                    }
                }
                timeChargeEnd -= step;
            }
            timeChargeEnd++; // inclusive

            timer.Stop();
            Console.WriteLine($"Solved in {timer.ElapsedMilliseconds}ms");
            return timeChargeEnd - timeChargeStart;
        }
    }
}
