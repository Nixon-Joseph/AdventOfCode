namespace AdventOfCode.Puzzles._2025.Day4;

public abstract class ForkliftBase : BaseSolution<RollGrid>
{
    internal override RollGrid ReadInputFromFile()
    {
        var arr = ReadInputFileAsCharArray("./Puzzles/2025/Day4/Input.txt");
        var grid = new RollGrid(arr.GetLength(0), arr.GetLength(1));
        for (var i = 0; i < arr.GetLength(0); i++)
        {
            for (var j = 0; j < arr.GetLength(1); j++)
            {
                if (arr[i, j] == '@')
                {
                    grid[i, j] = new Roll(i, j, ref grid);
                }
            }
        }
        return grid;
    }
}

public class RollGrid
{
    public RollGrid(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        Grid = new Roll[rows, cols];
    }

    public Roll this[int row, int col]
    {
        get { return Grid[row, col]; }
        set { Grid[row, col] = value; }
    }

    public int Rows { get; }
    public int Cols { get; }
    public Roll[,] Grid { get; }

    public int CountMovableRolls()
    {
        var count = 0;
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Cols; j++)
            {
                var roll = Grid[i, j];
                if (roll != null && roll.CanBeMoved)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public void RemoveMovableRolls()
    {
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Cols; j++)
            {
                var roll = Grid[i, j];
                if (roll != null)
                {
                    if (roll.CanBeMoved)
                    {
                        Grid[i, j] = null;
                    }
                    else
                    {
                        // reset CanBeMoved so it's recalculated next time
                        roll.ResetCanBeMoved();
                    }
                }
            }
        }
    }
}

public class Roll
{
    public Roll(int row, int col, ref RollGrid grid)
    {
        Row = row;
        Col = col;
        Grid = grid;
    }

    private readonly RollGrid Grid;
    public int Row { get; }
    public int Col { get; }
    private bool? _CanBeMoved = null;
    public bool CanBeMoved
    {
        get {
            if (_CanBeMoved.HasValue)
            {
                return _CanBeMoved.Value;
            }
            _CanBeMoved = DetermineIfCanBeMoved();
            return _CanBeMoved.Value;
        }
    }

    public void ResetCanBeMoved()
    {
        _CanBeMoved = null;
    }

    private bool DetermineIfCanBeMoved()
    {
        // A roll can be moved if it has fewer than 4 adjacent rolls in all 8 directions
        var directions = new (int dRow, int dCol)[]
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),           (0, 1),
            (1, -1),  (1, 0),  (1, 1)
        };
        var adjacentCount = 0;
        foreach (var (dRow, dCol) in directions)
        {
            var newRow = Row + dRow;
            var newCol = Col + dCol;
            if (newRow >= 0 && newRow < Grid.Rows && newCol >= 0 && newCol < Grid.Cols)
            {
                if (Grid.Grid[newRow, newCol] != null)
                {
                    adjacentCount++;
                }
            }
        }
        return adjacentCount < 4;
    }
}
