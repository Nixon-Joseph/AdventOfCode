using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles._2023.Day12
{
    internal class SpringSolution1 : SpringBase
    {
        public override object Solve()
        {
            var lines = ReadInputFromFile();
            var sum = 0;

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var springRow = parts[0];
                var counts = parts[1].Split(',').Select(int.Parse);
                sum += GetPossibleConfigurationCount(springRow, counts);
            }

            return sum;
        }


        /* -- EXAMPLE
            ???.### 1,1,3
            .??..??...?##. 1,1,3
            ?#?#?#?#?#?#?#? 1,3,1,6
            ????.#...#... 4,1,1
            ????.######..#####. 1,6,5
            ?###???????? 3,2,1
        */
        private static readonly Regex _consecutivePeriodRegex = new(@"[\.]+", RegexOptions.Compiled);

        private int GetPossibleConfigurationCount(string springRow, IEnumerable<int> counts)
        {
            var cleanLine = _consecutivePeriodRegex.Replace(springRow.Trim('.'), ".");
            // trim/regex allows this to work for cases like: `???.### 1,1,3` AND `????.#...#... 4,1,1`
            if (cleanLine.Length == counts.Count() - 1 + counts.Sum())
            {
                return 1;
            }
            else if (cleanLine.Contains('.')) //  ???.### 1,1,3 || .??..??...?##. 1,1,3 || ????.#...#... 4,1,1 || ????.######..#####. 1,6,5 || .#??.?.???? 2,1,1
            {
                // remember the above examples collapses periods to a single period,
                // so splitting on period will provide nice groups
                var groups = cleanLine.Split('.');
                // ????.#...#... becomes [????, #, #]
                if (counts.Count() == groups.Count() && groups.Sum(x => x.Length) == counts.Sum()) // ????.#...#... 4,1,1
                {
                    return 1;
                }
                else
                {
                    var countVariations = groups.Select(g => 0).ToArray();
                    for (int i = 0; i < groups.Count(); i++)
                    {
                        if (groups[i].Length == counts.ElementAt(i))
                        {
                            continue;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            else if (cleanLine.All(c => c == '?' || c == '#')) // ?#?#?#?#?#?#?#? 1,3,1,6 || ?###???????? 3,2,1
            {
                var possibleStartIndex = 0;
                for (int i = 0; i < counts.Count(); i++)
                {
                    var count = counts.ElementAt(i);
                    char? prev = i > 0 ? cleanLine[i - 1] : null;
                    char curr = cleanLine[i];
                    char? next = i < cleanLine.Length - 1 ? cleanLine[i + 1] : null;
                    if (cleanLine[possibleStartIndex + count] != '#')
                    {
                        possibleStartIndex += possibleStartIndex + count + 1;
                        break;
                    }
                    possibleStartIndex++;
                    continue;
                }
            }

            return 1;
        }
    }
}
