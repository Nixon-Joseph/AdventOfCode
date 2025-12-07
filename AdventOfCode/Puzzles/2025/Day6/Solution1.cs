namespace AdventOfCode.Puzzles._2025.Day6;

public class Solution1 : SolutionBase
{
    protected override object DoSolve()
    {
        var (operands, operators) = ReadInputFromFile();
        var loopLength = operators.Count();
        var sum = 0ul;
        for (int i = 0; i < loopLength; i++)
        {
            var op = operators.ElementAt(i);
            var vals = operands.Select(o => o.ElementAt(i));
            sum += op switch
            {
                '+' => vals.Aggregate(0ul, (a, b) => a + b),
                '*' => vals.Aggregate(1ul, (a, b) => a * b),
                _ => 0ul
            };
        }
        return sum;
    }
}
