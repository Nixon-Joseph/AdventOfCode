using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day05
{
    internal abstract class SeedsBase : BaseSolution<(IEnumerable<long> seeds, Mappers mappers)>
    {
        private readonly Regex mapRegex = new(@"^(?<mapName>[a-z]+\-to\-[a-z]+) map:");
        private readonly Regex mapContentsRegex = new(@"^(?<destination>[\d]+) (?<rangeStart>[\d]+) (?<rangeLength>[\d]+)");

        override internal (IEnumerable<long> seeds, Mappers mappers) ReadInputFromFile()
        {
            var seeds = new List<long>();
            var mappers = new Mappers();
            var lines = File.ReadAllLines(@"./Puzzles/2023/Day05/Input.txt");
            Mapper? currentMapper = null;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentMapper = null;
                    continue;
                }
                if (line.StartsWith("seeds: "))
                {
                    seeds.AddRange(line[7..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)));
                }
                else if (mapRegex.Match(line) is var mapMatch && mapMatch.Success)
                {
                    currentMapper = mapMatch.Groups["mapName"].Value switch
                    {
                        "seed-to-soil" => mappers.SeedToSoilMap,
                        "soil-to-fertilizer" => mappers.SoilToFertilizerMap,
                        "fertilizer-to-water" => mappers.FertilizerToWaterMap,
                        "water-to-light" => mappers.WaterToLightMap,
                        "light-to-temperature" => mappers.LightToTempteratureMap,
                        "temperature-to-humidity" => mappers.TemperatureToHumidityMap,
                        "humidity-to-location" => mappers.HumidityToLocationMap,
                        _ => throw new Exception("Unknown map name")
                    };
                }
                else if (mapContentsRegex.Match(line) is var contentMatch && contentMatch.Success && currentMapper is not null)
                {
                    currentMapper.AddMap(long.Parse(contentMatch.Groups["destination"].Value), long.Parse(contentMatch.Groups["rangeStart"].Value), long.Parse(contentMatch.Groups["rangeLength"].Value));
                }
            }
            return (seeds, mappers);
        }
    }

    internal class Mappers
    {
        public Mapper SeedToSoilMap { get; } = new();
        public Mapper SoilToFertilizerMap { get; } = new();
        public Mapper FertilizerToWaterMap { get; } = new();
        public Mapper WaterToLightMap { get; } = new();
        public Mapper LightToTempteratureMap { get; } = new();
        public Mapper TemperatureToHumidityMap { get; } = new();
        public Mapper HumidityToLocationMap { get; } = new();
    }

    internal class Mapper
    {
        private List<Map> _maps = new List<Map>();
        public long this[long index]
        {
            get {
                foreach (var map in _maps)
                {
                    if (index >= map.RangeStart && index < map.RangeStart + map.RangeLength)
                    {
                        return map.Destination + (index - map.RangeStart);
                    }
                }
                return index;
            }
        }

        public IEnumerable<Map> Maps => _maps;

        public void AddMap(long dest, long start, long length)
        {
            _maps.Add(new Map(dest, start, length));
            _maps = _maps.OrderBy(m => m.RangeStart).ToList();
        }
    }

    internal class Map
    {
        public Map(long dest, long start, long length)
        {
            Destination = dest;
            RangeStart = start;
            RangeLength = length;
        }

        public bool IsInDestinationInRange(long min, long max)
        {
            return (min >= Destination && min < Destination + RangeLength) ||
                    (max <= Destination + RangeLength && max >= Destination) ||
                    (min <= Destination && max >= Destination + RangeLength);
        }

        public (long availableMin, long availableMax) GetAvailableRange(long min, long max)
        {
            var availableMin = Math.Max(min, RangeStart);
            var availableMax = Math.Min(max, RangeStart + RangeLength);
            return (availableMin, availableMax);
        }

        public long Destination { get; set; }
        public long RangeStart { get; set; }
        public long RangeLength { get; set; }
    }
}
