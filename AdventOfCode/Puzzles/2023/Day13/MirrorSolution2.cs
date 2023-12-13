namespace AdventOfCode.Puzzles._2023.Day13
{
    internal class MirrorSolution2 : MirrorBase
    {
        protected override bool IsMirror(IEnumerable<string> puzzle, int mirrorTestIndex)
        {
            var mirrorPointDistance = 0;
            var foundSmudge = false;
            while (mirrorTestIndex - mirrorPointDistance >= 0 && mirrorTestIndex + 1 + mirrorPointDistance < puzzle.Count())
            {
                var a = puzzle.ElementAt(mirrorTestIndex - mirrorPointDistance);
                var b = puzzle.ElementAt(mirrorTestIndex + 1 + mirrorPointDistance);
                var matches = a == b;
                if (!foundSmudge && !matches)
                {
                    var numDifferences = GetNumberDifferences(a, b);
                    if (numDifferences == 1)
                    {
                        foundSmudge = true;
                        matches = true;
                    }
                }
                if (matches)
                {
                    mirrorPointDistance++;
                }
                else
                {
                    return false;
                }
            }
            return foundSmudge;
        }

        private int GetNumberDifferences(string a, string b)
        {
            var differences = 0;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    differences++;
            }
            return differences;
        }
    }
}
