namespace AdventOfCode.Puzzles._2023.Day05
{
    internal class SeedSolution1 : SeedsBase
    {
        public override object Solve()
        {
            var (seeds, mappers) = ReadInputFromFile();
            var minValue = long.MaxValue;

            foreach (var seed in seeds)
            {
                var soil = mappers.SeedToSoilMap[seed];
                var fertilizer = mappers.SoilToFertilizerMap[soil];
                var water = mappers.FertilizerToWaterMap[fertilizer];
                var light = mappers.WaterToLightMap[water];
                var temperature = mappers.LightToTempteratureMap[light];
                var humidity = mappers.TemperatureToHumidityMap[temperature];
                var location = mappers.HumidityToLocationMap[humidity];
                if (location < minValue)
                {
                    minValue = location;
                }
            }

            return minValue;
        }
    }
}
