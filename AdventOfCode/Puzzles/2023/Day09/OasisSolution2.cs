namespace AdventOfCode.Puzzles._2023.Day09
{
    internal class OasisSolution2 : OasisBase
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
                layers[^2].Insert(0, layers[^2].First()); // Add the first element of the second to last layer as the first element, skips the first step
                for (int i = layers.Count - 2; i >= 1; i--)
                {
                    layers[i - 1].Insert(0, layers[i - 1].First() - layers[i].First());
                }
                total += layers[0].First();
            }

            return total;
        }
    }
}
