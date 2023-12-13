namespace AdventOfCode.Puzzles._2023.Day13
{
    internal class MirrorSolution1 : MirrorBase
    {
        protected override bool IsMirror(IEnumerable<string> puzzle, int mirrorTestIndex)
        {
            var mirrorPointDistance = 0;
            while (mirrorTestIndex - mirrorPointDistance >= 0 && mirrorTestIndex + 1 + mirrorPointDistance < puzzle.Count())
            {
                if (puzzle.ElementAt(mirrorTestIndex - mirrorPointDistance) == puzzle.ElementAt(mirrorTestIndex + 1 + mirrorPointDistance))
                {
                    mirrorPointDistance++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
