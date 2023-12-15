namespace AdventOfCode.Puzzles.Day6
{
    public class CommunicationSolution1 : CommunicationSolutionBase
    {
        public CommunicationSolution1() : base(4) { }

        protected override object DoSolve()
        {
            var input = ReadInputFromFile();
            var queue = new Queue<char>();
            var processedChars = 0;
            foreach(var character in input)
            {
                processedChars++;
                queue.Enqueue(character);
                if (queue.Count() >= _charsToCheck)
                {
                    while (queue.Count() > _charsToCheck)
                        queue.Dequeue();
                    if (queue.Distinct().Count() == _charsToCheck)
                        break;
                }
            }
            return processedChars;
        }
    }
}
