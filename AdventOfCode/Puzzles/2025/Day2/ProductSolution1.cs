namespace AdventOfCode.Puzzles._2025.Day2
{
    public class ProductSolution1 : ProductBase
    {
        protected override object DoSolve()
        {
            var parts = ReadInputFromFile();
            ulong invalidIdSum = 0;
            foreach (var (low, high) in parts)
            {
                for (var i = low; i <= high; i++)
                {
                    var strVal = i.ToString();
                    if (strVal.Length % 2 == 0)
                    {
                        var mid = strVal.Length / 2;
                        var left = strVal.Substring(0, mid);
                        var right = strVal.Substring(mid);
                        if (left == right)
                        {
                            invalidIdSum += i;
                        }
                    }
                }
            }
            return invalidIdSum;
        }
    }
}
