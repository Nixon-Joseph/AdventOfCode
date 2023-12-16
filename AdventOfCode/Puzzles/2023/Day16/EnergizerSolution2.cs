namespace AdventOfCode.Puzzles._2023.Day16
{
    internal class EnergizerSolution2 : EnergizerBase
    {
        protected override object DoSolve()
        {
            var input = ReadInputFromFile();
            var maxResult = 0;
            var northChecks = Enumerable.Range(0, input.GetLength(1)).Select(x => new LightPath(x, 0, 'S')).ToList();
            var southChecks = Enumerable.Range(0, input.GetLength(1)).Select(x => new LightPath(x, input.GetLength(0) - 1, 'N')).ToList();
            var eastChecks = Enumerable.Range(0, input.GetLength(0)).Select(y => new LightPath(0, y, 'E')).ToList();
            var westChecks = Enumerable.Range(0, input.GetLength(0)).Select(y => new LightPath(input.GetLength(1) - 1, y, 'W')).ToList();
            var allChecks = northChecks.Concat(southChecks).Concat(eastChecks).Concat(westChecks).ToList();
            maxResult = allChecks.Max(l => GetNumEnergizedFromEntry(l));
            return maxResult;
        }
    }
}
