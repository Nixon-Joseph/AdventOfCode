namespace AdventOfCode.Puzzles._2023.Day07
{
    internal class CamelSolution1 : CamelCardBase
    {
        public CamelSolution1() : base(false) { }

        public override object Solve()
        {
            var hands = ReadInputFromFile();
            hands.Sort();
            var sum = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                sum += hands[i].Bid * (i + 1);
            }
            return sum; // 249483956
        }
    }
}
