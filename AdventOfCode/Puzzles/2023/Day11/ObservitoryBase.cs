namespace AdventOfCode.Puzzles._2023.Day11
{
    internal abstract class ObservitoryBase : BaseSolution<IEnumerable<Galaxy>>
    {
        protected int ExpansionFactor { get; }
        public ObservitoryBase(int expansionFactor)
        {
            ExpansionFactor = expansionFactor;
        }

        internal override IEnumerable<Galaxy> ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@".\Puzzles\2023\Day11\Input.txt").ToList();
            // handle expansions
            var emptyColumns = Enumerable.Range(0, lines[0].Length).ToList();
            var emptyRows = new List<int>();
            for (int i = 0; i < lines.Count; i++)
            {
                if (!lines[i].Contains('#'))
                {
                    emptyRows.Add(i);
                }
                else
                {
                    var foundGalaxies = lines[i].Select((c, index) => (c, index)).Where(x => x.c == '#').ToList();
                    foreach (var galaxy in foundGalaxies)
                    {
                        emptyColumns.Remove(galaxy.index);
                    }
                }
            }
            var galaxies = new List<Galaxy>();
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (lines[i][j] == '#')
                    {
                        galaxies.Add(new Galaxy(galaxies.Count, j, i, emptyColumns, emptyRows, ExpansionFactor));
                    }
                }
            }
            return galaxies;
        }

        public override object Solve()
        {
            var galaxies = ReadInputFromFile();
            foreach (var galaxy in galaxies)
            {
                foreach (var otherGalaxy in galaxies)
                {
                    if (galaxy.ID != otherGalaxy.ID)
                    {
                        galaxy.CalcDistanceToNeighbor(otherGalaxy);
                    }
                }
            }
            return Galaxy.NeighborDistances.Sum(x => x.Value);
        }
    }

    public class Galaxy
    {
        public Galaxy(int id, int x, int y, List<int> emptyColumns, List<int> emptyRows, int expansionFactor)
        {
            ID = id;
            var emptyColumnsBefore = emptyColumns.Where(c => c < x).Count();
            var emptyRowsBefore = emptyRows.Where(r => r < y).Count();
            X = emptyColumnsBefore * expansionFactor + x - emptyColumnsBefore;
            Y = emptyRowsBefore * expansionFactor + y - emptyRowsBefore;
        }

        public int ID { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
        public static Dictionary<string, long> NeighborDistances { get; } = new Dictionary<string, long>();

        public void CalcDistanceToNeighbor(Galaxy neighbor)
        {
            var key = GetKey(ID, neighbor.ID);
            if (!NeighborDistances.ContainsKey(key))
            {
                var distance = Math.Abs(X - neighbor.X) + Math.Abs(Y - neighbor.Y);
                NeighborDistances.Add(key, distance);
            }
        }

        private static string GetKey(int galaxy1, int galaxy2)
        {
            return $"{Math.Min(galaxy1, galaxy2)}-{Math.Max(galaxy1, galaxy2)}";
        }
    }
}
