namespace AdventOfCode.Puzzles.Day3
{
    public class RucksackSolution1 : ElfRucksackSolutionBase
    {
        public override object Solve()
        {
            var rucksacks = ReadInputFromFile();
            var prioritySum = 0;
            foreach( var sack in rucksacks ) {
                if (sack.Compartment1.Items.FirstOrDefault(x => sack.Compartment2.Items.Any(y => y.Code == x.Code)) is Rucksack.Compartment.Item foundItem1)
                    prioritySum += foundItem1.Value;
                else if (sack.Compartment2.Items.FirstOrDefault(x => sack.Compartment1.Items.Any(y => y.Code == x.Code)) is Rucksack.Compartment.Item foundItem2)
                    prioritySum += foundItem2.Value;
            }
            return prioritySum;
        }
    }
}
