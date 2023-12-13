namespace AdventOfCode.Puzzles._2023.Day13
{
    internal abstract class MirrorBase : BaseSolution<List<List<string>>>
    {
        internal override List<List<string>> ReadInputFromFile()
        {
            var lines = File.ReadAllLines(@".\Puzzles\2023\Day13\Input.txt");
            var puzzles = new List<List<string>>();
            var curPuzzle = new List<string>();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    puzzles.Add(curPuzzle);
                    curPuzzle = new List<string>();
                }
                else
                {
                    curPuzzle.Add(line);
                }
            }
            puzzles.Add(curPuzzle);
            return puzzles;
        }

        public override object Solve()
        {
            var puzzles = ReadInputFromFile();
            var sum = 0;

            var hitPuzzles = new List<int>();
            var puzzleIndex = 0;
            foreach (var puzzle in puzzles)
            {
                var iteration = 0;
                var isMirror = false;
                do
                {
                    var isVerticalCheck = iteration != 0;
                    var workingPuzzle = isVerticalCheck ? puzzle : RotatePuzzle(puzzle);

                    var reflectionPoint = 0;
                    while (reflectionPoint < workingPuzzle.Count - 1)
                    {
                        isMirror = IsMirror(workingPuzzle, reflectionPoint);
                        if (isMirror)
                        {
                            hitPuzzles.Add(puzzleIndex);
                            sum += (reflectionPoint + 1) * (isVerticalCheck ? 100 : 1);
                            break;
                        }
                        reflectionPoint += 1;
                    }
                    iteration++;
                }
                while (!isMirror && iteration < 2);

                puzzleIndex++;
            }
            return sum;
        }

        protected abstract bool IsMirror(IEnumerable<string> puzzle, int mirrorTestIndex);

        protected static List<string> RotatePuzzle(List<string> puzzle)
        {
            var rotatedPuzzle = new List<string>();
            for (int i = 0; i < puzzle[0].Length; i++)
            {
                var rotated = "";
                for (int j = puzzle.Count - 1; j >= 0; j--)
                {
                    rotated += puzzle[j][i];
                }
                rotatedPuzzle.Add(rotated);
            }
            return rotatedPuzzle;
        }
    }
}
