namespace AdventOfCode.Puzzles.Day5
{
    internal class CrateSolution2 : StackMoverSolutionBase
    {
        protected override object DoSolve()
        {
            var stacks = ReadInputFromFile();
            foreach (var instruction in stacks.Instructions)
            {
                var cratesMoved = 0;
                var tmpStack = new Stack<char>();
                while (cratesMoved < instruction.MoveCount && stacks.ElementAt(instruction.MoveFrom - 1).Any())
                {
                    var crate = stacks.ElementAt(instruction.MoveFrom - 1).Pop();
                    tmpStack.Push(crate);
                    cratesMoved++;
                }
                while (tmpStack.Any())
                {
                    stacks.ElementAt(instruction.MoveTo - 1).Push(tmpStack.Pop());
                }
            }
            return GetStackTops(stacks);
        }
    }
}
