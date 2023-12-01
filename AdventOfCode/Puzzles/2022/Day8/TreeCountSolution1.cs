namespace AdventOfCode.Puzzles.Day8
{
    public class TreeCountSolution1 : TreeCountBase
    {
        public override object Solve()
        {
            var (rowCount, colCount, grid) = ReadInputFromFile();
            var visibleTreeCount = 0;
            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    var isVisible = scanVisible(row, col, grid, rowCount, colCount);
                    if (isVisible)
                    {
                        visibleTreeCount++;
                    }
                }
                if (row % 5 == 0) {
                    Console.WriteLine($"Row {row+1} of {rowCount}");
                }
            }
            return visibleTreeCount;
        }

        private static bool scanVisible(int row, int col, GridTree[,] treeGridGrid, int rowCount, int colCount)
        {
            var tree = treeGridGrid[row, col];
            var scanRow = row;
            var scanCol = col;
            (bool top, bool left, bool right, bool bottom) visibility = (true, true, true, true);
            if (row == 0 || row == rowCount - 1 || col == 0 || col == colCount - 1)
                return true; // on the edge, so visible
            
            while (scanRow > 0) // scan up
                if (treeGridGrid[--scanRow, scanCol].Height >= tree.Height) { visibility.top = false; break; }
            if (visibility.top) return true; // breakout if already visible
            scanRow = row;
            while (scanRow < rowCount - 1) // scan down
                if (treeGridGrid[++scanRow, scanCol].Height >= tree.Height) { visibility.bottom = false; break; }
            if (visibility.bottom) return true; // breakout if already visible
            scanRow = row;
            while (scanCol > 0) // scan left
                if (treeGridGrid[scanRow, --scanCol].Height >= tree.Height) { visibility.left = false; break; }
            if (visibility.left) return true; // breakout if already visible
            scanCol = col;
            while (scanCol < colCount - 1) // scan right
                if (treeGridGrid[scanRow, ++scanCol].Height >= tree.Height) { visibility.right = false; break; }
            return visibility.top || visibility.right || visibility.left || visibility.bottom;
        }
    }
}
