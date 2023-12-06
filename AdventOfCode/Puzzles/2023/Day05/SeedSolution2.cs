namespace AdventOfCode.Puzzles._2023.Day05
{
    internal class SeedSolution2 : SeedsBase
    {
        public override object Solve()
        {
            var (seeds, mappers) = ReadInputFromFile();

            var maps = mappers.HumidityToLocationMap.Maps.OrderBy(m => m.Destination).ToList();
            var seedPairs = new List<(long min, long max)>();
            var minValue = long.MaxValue;
            for (int i = 0; i < seeds.Count() - 1; i += 2)
            {
                seedPairs.Add((seeds.ElementAt(i), seeds.ElementAt(i) + seeds.ElementAt(i + 1)));
            }
            var locMap = maps.First();
            var validLocStart = locMap.RangeStart;
            var validLocEnd = locMap.RangeStart + locMap.RangeLength;
            foreach (var humidMap in mappers.TemperatureToHumidityMap.Maps)
            {
                if (humidMap.IsInDestinationInRange(validLocStart, validLocEnd))
                {
                    var validHumidStart = humidMap.RangeStart;
                    var validHumidEnd = humidMap.RangeStart + humidMap.RangeLength;
                    foreach (var tempMap in mappers.LightToTempteratureMap.Maps)
                    {
                        if (tempMap.IsInDestinationInRange(validHumidStart, validHumidEnd))
                        {
                            var validTempStart = tempMap.RangeStart;
                            var validTempEnd = tempMap.RangeStart + tempMap.RangeLength;
                            foreach (var lightMap in mappers.FertilizerToWaterMap.Maps)
                            {
                                if (lightMap.IsInDestinationInRange(validTempStart, validTempEnd))
                                {
                                    var validLightStart = lightMap.RangeStart;
                                    var validLightEnd = lightMap.RangeStart + lightMap.RangeLength;
                                    foreach (var fertMap in mappers.SoilToFertilizerMap.Maps)
                                    {
                                        if (fertMap.IsInDestinationInRange(validLightStart, validLightEnd))
                                        {
                                            var validFertStart = fertMap.RangeStart;
                                            var validFertEnd = fertMap.RangeStart + fertMap.RangeLength;
                                            foreach (var seedMap in mappers.SeedToSoilMap.Maps)
                                            {
                                                if (seedMap.IsInDestinationInRange(validFertStart, validFertEnd))
                                                {
                                                    var validSeedStart = seedMap.RangeStart;
                                                    var validSeedEnd = seedMap.RangeStart + seedMap.RangeLength;
                                                    var seedPairIndex = 1;
                                                    foreach (var pair in seedPairs)
                                                    {
                                                        Console.WriteLine($"Starting seedPair: {seedPairIndex}");
                                                        if (
                                                            (pair.min >= validSeedStart && pair.min < validSeedEnd) ||
                                                            (pair.max <= validSeedEnd && pair.max > validSeedStart) ||
                                                            (pair.min <= validSeedStart && pair.max >= validSeedEnd)
                                                        )
                                                        {
                                                            var start = pair.min;
                                                            var end = pair.max;
                                                            var high = 0L;
                                                            for (var i = start; i < end; i += 5000)
                                                            {
                                                                var newVal = GetLocationBySeed(i, mappers);
                                                                if (minValue > newVal)
                                                                {
                                                                    minValue = newVal;
                                                                    high = i;
                                                                }
                                                            }

                                                            var low = 0L;
                                                            for (var i = high; i > validLocStart; i -= 500)
                                                            {
                                                                var newVal = GetLocationBySeed(i, mappers);
                                                                if (minValue > newVal)
                                                                {
                                                                    minValue = newVal;
                                                                }
                                                                if (newVal > minValue)
                                                                {
                                                                    high = i + 500;
                                                                    low = i - 500;
                                                                    break;
                                                                }
                                                            }

                                                            for (var i = low; i < high; i++)
                                                            {
                                                                var newVal = GetLocationBySeed(i, mappers);
                                                                if (minValue > newVal)
                                                                {
                                                                    minValue = newVal;
                                                                }
                                                            }
                                                        }
                                                        Console.WriteLine($"Ending seedPair: {seedPairIndex++}");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return minValue;
        }

        private long GetLocationBySeed(long seed, Mappers mappers)
        {
            var soil = mappers.SeedToSoilMap[seed];
            var fertilizer = mappers.SoilToFertilizerMap[soil];
            var water = mappers.FertilizerToWaterMap[fertilizer];
            var light = mappers.WaterToLightMap[water];
            var temperature = mappers.LightToTempteratureMap[light];
            var humidity = mappers.TemperatureToHumidityMap[temperature];
            var location = mappers.HumidityToLocationMap[humidity];
            return location;
        }
    }
}
