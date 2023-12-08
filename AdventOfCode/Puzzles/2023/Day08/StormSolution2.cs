using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023.Day08
{
    internal class StormSolution2 : StormBase
    {
        public override object Solve()
        {
            var (directions, nodeDict) = ReadInputFromFile();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var iterationCounter = 0;
            var selectedNodes = nodeDict.Values.Where(n => n.Name.EndsWith('A')).ToList();

            var skipDict = new Dictionary<string, Node>();

            while (!selectedNodes.All(n => n.Name.EndsWith('Z')))
            {
                var startNodes = selectedNodes.Select(n => n.Name).ToArray();
                if (startNodes.All(s => skipDict.ContainsKey(s)))
                {
                    selectedNodes = startNodes.Select(s => skipDict[s]).ToList();
                }
                else 
                {
                    foreach (var direction in directions)
                    {
                        for (int i = 0; i < selectedNodes.Count; i++)
                        {
                            selectedNodes[i] = selectedNodes[i][direction];
                        }
                    }
                }
                for (var i = 0; i < selectedNodes.Count; i++)
                {
                    if (!skipDict.ContainsKey(startNodes[i]))
                    {
                        skipDict[startNodes[i]] = selectedNodes[i];
                    }
                }
                // found all paths this is going to travel through
                if (skipDict.Values.All(v => skipDict.ContainsKey(v.Name)))
                {
                    break;
                }
                iterationCounter++;
            }
            var superSkipDict = skipDict.Where(n => n.Key.EndsWith('A')).ToDictionary(s => s.Key, s => 0UL);
            foreach (var nodeKVP in skipDict.Where(n => n.Key.EndsWith('A')))
            {
                var tmpNode = nodeDict[nodeKVP.Key];
                var distance = 0UL;
                while (true)
                {
                    tmpNode = skipDict[tmpNode.Name];
                    distance += (ulong)directions.Count();
                    if (tmpNode.Name.EndsWith('Z'))
                    {
                        superSkipDict[nodeKVP.Key] = distance;
                        break;
                    }
                }
            }
            // use least common denominator to find the least number of steps to get to where all nodes end with a Z
            var lcm = superSkipDict.Values.Select(v => v).Aggregate((S, val) => S * val / GCD(S, val));

            stopWatch.Stop();
            Console.WriteLine($"Solved in {stopWatch.ElapsedMilliseconds}ms");
            return lcm;
        }

        private static ulong GCD(ulong a, ulong b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
