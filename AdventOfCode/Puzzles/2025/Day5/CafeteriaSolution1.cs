namespace AdventOfCode.Puzzles._2025.Day5;

public class CafeteriaSolution1 : CafeteriaBase
{
    protected override object DoSolve()
    {
        var inv = ReadInputFromFile();
        var freshCount = 0;
        foreach (var id in inv.IngredientIDs)
        {
            foreach (var (min, max) in inv.FreshIngredientRanges)
            {
                if (id >= min && id <= max)
                {
                    freshCount++;
                    break;
                }
            }
        }
        return freshCount;
    }
}
