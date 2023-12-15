namespace AdventOfCode.Puzzles.Day5
{
    internal class CrateSolution1 : StackMoverSolutionBase
    {
        protected override object DoSolve()
        {
            var stacks = ReadInputFromFile();
            foreach (var instruction in stacks.Instructions)
            {
                var cratesMoved = 0;
                while (cratesMoved < instruction.MoveCount && stacks.ElementAt(instruction.MoveFrom - 1).Any())
                {
                    var crate = stacks.ElementAt(instruction.MoveFrom - 1).Pop();
                    stacks.ElementAt(instruction.MoveTo - 1).Push(crate);
                    cratesMoved++;
                }
            }
            return GetStackTops(stacks);
        }
    }
}
