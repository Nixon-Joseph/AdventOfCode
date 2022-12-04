namespace AdventOfCode.Puzzles.Day3
{
    public abstract class ElfRucksackSolutionBase : BaseSolution<IEnumerable<Rucksack>>
    {
        internal override IEnumerable<Rucksack> ReadInputFromFile()
        {
            var fileLines = File.ReadLines("./Puzzles/Day3/Input.txt");
            return fileLines.Select(x => new Rucksack(x));
        }
    }

    public class Rucksack
    {
        public Compartment Compartment1 { get; set; }
        public Compartment Compartment2 { get; set; }

        public Rucksack(string contents) {
            var compartment1Items = contents.Substring(0, (int)(contents.Length / 2));
            var compartment2Items = contents.Substring((int)(contents.Length / 2), (int)(contents.Length / 2));
            Compartment1 = new Compartment(compartment1Items);
            Compartment2 = new Compartment(compartment2Items);
        }

        public IEnumerable<Compartment.Item> GetAllItems() => Compartment1.Items.Concat(Compartment2.Items);

        public class Compartment
        {
            public IEnumerable<Item> Items { get; set; }

            public Compartment(string items)
            {
                Items = items.Select(x => new Item(x));
            }

            public class Item
            {
                private const string ALL_CAPS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

                public char Code { get; set; }
                public short Value => (short)(ALL_CAPS.Contains(Code) ? (int)Code - 38 : (int)Code - 96);

                public Item(char code) => Code = code;

                public override bool Equals(object? obj) => obj is Item compare && compare.Code == Code;
            }
        }
    }
}
