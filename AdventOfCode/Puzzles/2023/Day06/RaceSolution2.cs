using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023.Day06
{
    internal class RaceSolution2 : BaseSolution<Race>
    {
        internal override Race ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@"./Puzzles/2023/Day06/Input.txt");
            var time = 0L;
            var distance = 0L;
            foreach (var line in lines)
            {
                if (line.StartsWith("Time: "))
                {
                    time = long.Parse(string.Join("", line[6..].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
                }
                else if (line.StartsWith("Distance: "))
                {
                    distance = long.Parse(string.Join("", line[10..].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
                }
            }
            return new Race { Time = time, Distance = distance };
        }

        public override object Solve()
        {
            var timer = new Stopwatch();
            timer.Start();
            var race = ReadInputFromFile();

            var raceWinOptions = 0;
            var timeCharge = 1;
            while (timeCharge < race.Time)
            {
                if (timeCharge * (race.Time - timeCharge) > race.Distance)
                {
                    raceWinOptions++;
                }
                timeCharge++;
            }

            timer.Stop();
            Console.WriteLine($"Solved in {timer.ElapsedMilliseconds}ms");
            return raceWinOptions;
        }
    }
}
