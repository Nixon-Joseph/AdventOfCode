namespace AdventOfCode.Puzzles._2025.Day6;

public class Solution2 : SolutionBase
{
    protected override object DoSolve()
    {
        var arr = ReadInputAsCharArraysFromFile();
        var rotated = RotateMatrixClockwise(arr); // rotate array so it can be easily traversed bottom-up
        var sum = 0ul; // total sum for ultimate answer
        var n = rotated.GetLength(0); // height of rotated array
        var w = rotated.GetLength(1); // width of rotated array
        var operands = new List<ulong>(); // operands collected for current operation
        var numToCalc = ""; // current number being built from characters
        char? op = null; // current operator
        // traverse rotated array bottom-up
        for (int i = n - 1; i >= -1; i--)
        {
            // at the end of each row, finalize any pending number and perform calculation if operator is present
            if (!string.IsNullOrEmpty(numToCalc))
            {
                operands.Add(ulong.Parse(numToCalc));
                numToCalc = "";
            }
            if (op != null)
            {
                var tmpVal = 0ul;
                if (op == '+')
                {
                    tmpVal = operands.Aggregate(0ul, (a, b) => a + b);
                }
                else if (op == '*')
                {
                    tmpVal = operands.Aggregate(1ul, (a, b) => a * b);
                }
                // Console.WriteLine($"Calculated {tmpVal} for operator {op}, from operands {string.Join(", ", operands)}");
                sum += tmpVal;
                operands.Clear();
                op = null;
                continue; // this row is blank, continue to next row
            }
            // traverse each character in the row from right to left
            for (int j = w -1; j >= 0; j--)
            {
                var c = rotated[i, j];
                if (c != ' ')
                {
                    if (c == '+' || c == '*')
                    {
                        op = c;
                    }
                    else
                    {
                        numToCalc += c;
                    }
                }
            }
        }
        return sum;
    }

    public static char[,] RotateMatrixClockwise(char[,] matrix)
    {
        int w = matrix.GetLength(0); // Assuming a square matrix (n x n)
        int h = matrix.GetLength(1);

        char[,] rotated = new char[h, w];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                rotated[j, w - 1 - i] = matrix[i, j];
            }
        }
        return rotated;
    }
}
