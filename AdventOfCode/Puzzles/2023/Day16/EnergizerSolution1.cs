namespace AdventOfCode.Puzzles._2023.Day16
{
    internal class EnergizerSolution1 : EnergizerBase
    {
        protected override object DoSolve()
        {
            return GetNumEnergizedFromEntry(new LightPath(0, 0, 'E'));
        }
    }
}
