namespace AdventOfCode.Puzzles._2025.Day4;

public class ForkliftSolution2 : ForkliftBase
{
    protected override object DoSolve()
    {
        var grid = ReadInputFromFile();
        var totalMovable = 0;
        var movable = grid.CountMovableRolls();
        while (movable > 0)
        {
            totalMovable += movable;
            grid.RemoveMovableRolls();
            movable = grid.CountMovableRolls();
        }
        return totalMovable;
    }
}
