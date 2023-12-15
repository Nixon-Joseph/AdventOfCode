namespace AdventOfCode.Puzzles._2023.Day02
{
    public class CubeSolution2 : CubeBase
    {
        protected override object DoSolve()
        {
            var cubeGames = ReadInputFromFile();
            var sum = 0;
            foreach (var game in cubeGames)
            {
                var bluePower = 0;
                var redPower = 0;
                var greenPower = 0;
                foreach( var gameCubeSet in game.CubeSets ) {
                    if (gameCubeSet.ContainsKey(Color.blue)) { bluePower = Math.Max(bluePower, gameCubeSet[Color.blue]); }
                    if (gameCubeSet.ContainsKey(Color.red)) { redPower = Math.Max(redPower, gameCubeSet[Color.red]); }
                    if (gameCubeSet.ContainsKey(Color.green)) { greenPower = Math.Max(greenPower, gameCubeSet[Color.green]); }
                }
                sum += (bluePower * redPower * greenPower);
            }
            return sum;
        }
    }
}
