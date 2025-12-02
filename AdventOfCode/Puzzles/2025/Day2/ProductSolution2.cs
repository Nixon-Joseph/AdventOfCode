namespace AdventOfCode.Puzzles._2025.Day2
{
    public class ProductSolution2 : ProductBase
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
                    var stillValid = true;
                    var workStr = strVal[..1];
                    while (stillValid && workStr.Length < strVal.Length / 2 + 1)
                    {
                        if (strVal.Length % workStr.Length == 0)
                        {
                            var repeated = string.Concat(Enumerable.Repeat(workStr, strVal.Length / workStr.Length));
                            if (repeated == strVal)
                            {
                                stillValid = false;
                                invalidIdSum += i;
                                break;
                            }
                        }
                        workStr += strVal[workStr.Length];
                    }
                }
            }
            return invalidIdSum;
        }
    }
}
