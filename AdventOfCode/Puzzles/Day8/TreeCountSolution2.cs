namespace AdventOfCode.Puzzles.Day8
{
    public class TreeCountSolution2 : TreeCountBase
    {
        public override object Solve()
        {
            var (rowCount, colCount, grid) = ReadInputFromFile();
            var highScore = 0;
            for (var row = 0; row < rowCount; row++)
            {
                for (var col = 0; col < colCount; col++)
                {
                    var score = getScenicScore(row, col, grid, rowCount, colCount);
                    highScore = Math.Max(highScore, score);
                }
            }
            return highScore;
        }

        private static int getScenicScore(int row, int col, GridTree[,] treeGridGrid, int rowCount, int colCount)
        {
            if (row == 0 || row == rowCount - 1 || col == 0 || col == colCount - 1)
                return 0; // on the edge, so at least one multiplier == 0
            var tree = treeGridGrid[row, col];
            var scanRow = row;
            var scanCol = col;
            (int top, int left, int right, int bottom) dirScores = (0, 0, 0, 0);

            while (scanRow > 0) // scan up
                if (treeGridGrid[--scanRow, scanCol].Height >= tree.Height || scanRow == 0) { dirScores.top = row - scanRow; break; }
            scanRow = row;
            while (scanRow < rowCount - 1) // scan down
                if (treeGridGrid[++scanRow, scanCol].Height >= tree.Height || scanRow == rowCount - 1) { dirScores.bottom = scanRow - row; break; }
            scanRow = row;
            while (scanCol > 0) // scan left
                if (treeGridGrid[scanRow, --scanCol].Height >= tree.Height || scanCol == 0) { dirScores.left = col - scanCol; break; }
            scanCol = col;
            while (scanCol < colCount - 1) // scan right
                if (treeGridGrid[scanRow, ++scanCol].Height >= tree.Height || scanCol == colCount - 1) { dirScores.right = scanCol - col; break; }
            return dirScores.top * dirScores.right * dirScores.left * dirScores.bottom;
        }
    }
}
