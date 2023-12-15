namespace AdventOfCode.Puzzles._2023.Day06
{
    internal class RaceSolution1 : BaseSolution<IEnumerable<Race>>
    {
        internal override List<Race> ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@"./Puzzles/2023/Day06/Input.txt");
            var times = new List<long>();
            var distances = new List<long>();
            foreach (var line in lines)
            {
                if (line.StartsWith("Time: "))
                {
                    times.AddRange(line[6..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)));
                }
                else if (line.StartsWith("Distance: "))
                {
                    distances.AddRange(line[10..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)));
                }
            }
            var races = new List<Race>();
            for (int i = 0; i < times.Count; i++)
            {
                races.Add(new Race { Time = times[i], Distance = distances[i] });
            }
            return races;
        }

        protected override object DoSolve()
        {
            var races = ReadInputFromFile();
            var product = 1L;

            foreach (var race in races)
            {
                var raceWinOptions = 0L;
                var timeCharge = 1;
                while (timeCharge < race.Time)
                {
                    if (timeCharge * (race.Time - timeCharge) > race.Distance)
                    {
                        raceWinOptions++;
                    }
                    timeCharge++;
                }
                product *= raceWinOptions;
            }

            return product;
        }
    }
}
