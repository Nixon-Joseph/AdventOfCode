using MoreLinq;

namespace AdventOfCode.Puzzles.Day3
{
    public class RucksackSolution2 : ElfRucksackSolutionBase
    {
        public override object Solve()
        {
            var rucksacks = ReadInputFromFile();
            var prioritySum = 0;
            var sackGroups = rucksacks.Batch(3);
            foreach(var sackGroup in sackGroups) {
                var commonItems = new List<Rucksack.Compartment.Item>();
                foreach(var sack in sackGroup) {
                    if (!commonItems.Any()) // first go-round
                        commonItems.AddRange(sack.GetAllItems());
                    else
                        for (int i = commonItems.Count - 1; i >= 0; i--)
                        {
                            if (!sack.GetAllItems().Any(x => x.Code == commonItems[i].Code))
                                commonItems.RemoveAt(i);
                        }
                }
                if (commonItems.Any())
                    prioritySum += commonItems.First().Value;
            }
            return prioritySum;
        }
    }
}
