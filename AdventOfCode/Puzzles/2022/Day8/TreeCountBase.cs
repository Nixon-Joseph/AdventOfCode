namespace AdventOfCode.Puzzles.Day8
{
    public abstract class TreeCountBase : BaseSolution<(int rowCount, int colCount, GridTree[,] grid)>
    {
        internal override (int rowCount, int colCount, GridTree[,] grid) ReadInputFromFile()
        {
            var fileRows = File.ReadAllLines("./Puzzles/Day8/input.txt");
            var rowCount = fileRows.Length;
            var colCount = fileRows.First().Length;
            var treeGrid = new GridTree[rowCount, colCount];
            for (var row = 0; row < fileRows.Length; row++)
            {
                var fileRow = fileRows[row];
                for (var col = 0; col < fileRow.Length; col++)
                {
                    var treeHeight = fileRow[col].ToString();
                    treeGrid[row, col] = new GridTree(int.Parse(treeHeight));
                }
            }
            return (rowCount, colCount, treeGrid);
        }
    }

    public class GridTree
    {
        public GridTree(int height) => Height = height;
        public int Height { get; set; }
    }
}
