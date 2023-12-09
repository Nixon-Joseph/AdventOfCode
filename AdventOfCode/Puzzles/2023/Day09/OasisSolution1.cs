namespace AdventOfCode.Puzzles._2023.Day09
{
    internal class OasisSolution1 : OasisBase
    {
        public override object Solve()
        {
            var lists = ReadInputFromFile();
            var total = 0;

            foreach (var list in lists)
            {
                var layers = new List<List<int>> { list };
                var curLayer = list;
                List<int> curCollection = list;
                do
                {
                    curLayer = curCollection;
                    curCollection = new List<int>();
                    for (int i = 0; i < curLayer.Count - 1; i++)
                    {
                        curCollection.Add(curLayer[i + 1] - curLayer[i]);
                    }
                    layers.Add(curCollection);
                } while (!curCollection.All(x => x == 0));
                layers[^2].Add(layers[^2].Last()); // Add the last element of the second to last layer to the last layer, skips the first step
                for (int i = layers.Count - 2; i >= 1; i--)
                {
                    layers[i - 1].Add(layers[i - 1].Last() + layers[i].Last());
                }
                total += layers[0].Last();
            }

            return total;
        }
    }
}
