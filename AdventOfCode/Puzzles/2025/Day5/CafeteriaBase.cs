namespace AdventOfCode.Puzzles._2025.Day5;

public abstract class CafeteriaBase : BaseSolution<CafeteriaInventory>
{
    internal override CafeteriaInventory ReadInputFromFile()
    {
        var input = File.ReadAllLines("./Puzzles/2025/Day5/Input.txt");
        var inv = new CafeteriaInventory();
        var freshRanges = new List<(ulong min, ulong max)>();
        var ingredientIDs = new List<ulong>();
        var readingFreshRanges = true;
        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                readingFreshRanges = false;
                continue;
            }
            if (readingFreshRanges)
            {
                var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);
                var min = ulong.Parse(parts[0]);
                var max = ulong.Parse(parts[1]);
                freshRanges.Add((min, max));
            }
            else
            {
                ingredientIDs.Add(ulong.Parse(line));
            }
        }
        inv.FreshIngredientRanges = freshRanges;
        inv.IngredientIDs = ingredientIDs;
        return inv;
    }
}

public class CafeteriaInventory
{
    public IEnumerable<(ulong min, ulong max)> FreshIngredientRanges { get; set; }
    public IEnumerable<ulong> IngredientIDs { get; set; }
}
