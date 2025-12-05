namespace AdventOfCode.Puzzles._2025.Day5;

public class CafeteriaSolution2 : CafeteriaBase
{
    protected override object DoSolve()
    {
        var inv = ReadInputFromFile();
        var allPossibleFreshIDsCount = new HashSet<ulong>();
        // collapse all ranges into as few as possible
        var collapsedRanges = new List<(ulong start, ulong end)>();
        foreach (var (min, max) in inv.FreshIngredientRanges.OrderBy(r => r.min))
        {
            if (collapsedRanges.Count == 0)
            {
                collapsedRanges.Add((min, max));
                continue;
            }
            var foundOverlap = false;
            for (int i = 0; i < collapsedRanges.Count; i++)
            {
                var (cMin, cMax) = collapsedRanges[i];
                if (min >= cMin && min <= cMax)
                {
                    // overlap at start
                    collapsedRanges[i] = (cMin, Math.Max(cMax, max));
                    foundOverlap = true;
                    break;
                }
                else if (max >= cMin && max <= cMax)
                {
                    // overlap at end
                    collapsedRanges[i] = (Math.Min(cMin, min), cMax);
                    foundOverlap = true;
                    break;
                }
                else if (min <= cMin && max >= cMax)
                {
                    // engulfed
                    collapsedRanges[i] = (min, max);
                    foundOverlap = true;
                    break;
                }
            }
            if (!foundOverlap)
            {
                collapsedRanges.Add((min, max));
            }
        }
        var freshCount = 0ul;
        foreach (var (min, max) in collapsedRanges)
        {
            freshCount += max - min + 1ul;
        }
        return freshCount;
    }
}
