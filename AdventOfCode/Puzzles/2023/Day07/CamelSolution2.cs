namespace AdventOfCode.Puzzles._2023.Day07
{
    internal class CamelSolution2 : CamelCardBase
    {
        public CamelSolution2() : base(true) { }

        protected override object DoSolve()
        {
            var hands = ReadInputFromFile();
            hands.Sort();
            var sum = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                sum += hands[i].Bid * (i + 1);
            }
            return sum; // 252137472
        }
    }
}
