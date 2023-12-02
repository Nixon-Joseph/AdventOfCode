namespace AdventOfCode.Puzzles._2023.Day02
{
    public class CubeSolution1 : CubeBase
    {
        private const int SOLUTION_GREEN_COUNT = 13;
        private const int SOLUTION_RED_COUNT = 12;
        private const int SOLUTION_BLUE_COUNT = 14;

        public override object Solve()
        {
            var cubeGames = ReadInputFromFile();
            var sum = 0;
            foreach(var game in cubeGames)
            {
                if (game.CubeSets.Any(cs => cs.ContainsKey(Color.green) && cs[Color.green] > SOLUTION_GREEN_COUNT)) { continue; }
                if (game.CubeSets.Any(cs => cs.ContainsKey(Color.red) && cs[Color.red] > SOLUTION_RED_COUNT)) { continue; }
                if (game.CubeSets.Any(cs => cs.ContainsKey(Color.blue) && cs[Color.blue] > SOLUTION_BLUE_COUNT)) { continue; }
                sum += game.GameID;
            }
            return sum;
        }
    }
}
