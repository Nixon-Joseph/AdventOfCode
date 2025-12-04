namespace AdventOfCode.Puzzles._2025.Day4;

public class ForkliftSolution1 : ForkliftBase
{
    protected override object DoSolve()
    {
        var grid = ReadInputFromFile();
        return grid.CountMovableRolls();
    }
}
