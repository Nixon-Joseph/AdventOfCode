using MoreLinq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Puzzles.Day11
{
    internal class MonkeySolution : BaseSolution<List<Monkey>>
    {
        internal override List<Monkey> ReadInputFromFile()
        {
            var monkeyBatches = File.ReadLines("./Puzzles/Day11/Input.txt").Batch(7);
            var monkeys = new List<Monkey>();
            foreach (var monkeyBatch in monkeyBatches)
            {
                var monkey = new Monkey();
                monkey.ParseBatch(monkeyBatch);
                monkeys.Add(monkey);
            }
            return monkeys;
        }

        public override object Solve()
        {
            var monkeys = ReadInputFromFile();
            const int rounds = 10000;
            var currentRound = 0;
            while (currentRound++ < rounds)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.InspectItems(monkeys);
                }
                if (currentRound % 100 == 0)
                    Console.WriteLine($"Round {currentRound}");
            }
            BigInteger totalMonkeyBusiness = BigInteger.One;
            foreach (var topMonkey in monkeys.OrderByDescending(m => m.Inspections).Take(2))
            {
                totalMonkeyBusiness *= new BigInteger(topMonkey.Inspections);
            }
            return totalMonkeyBusiness.ToString();
        }
    }

    internal class Monkey
    {
        public int Inspections { get; set; }
        public List<BigInteger> Items { get; set; }
        public Func<BigInteger, BigInteger> WorryOperation { get; set; }
        public Action<BigInteger, List<Monkey>> Test { get; set; }

        public void InspectItems(List<Monkey> allMonkeys)
        {
            foreach (var item in Items)
            {
                Test(WorryOperation(item), allMonkeys);
                Inspections++;
            }
            Items.Clear();
        }

        public void ParseBatch(IEnumerable<string> monkeyBatch)
        {
            if (monkeyBatch.Count() < 6)
                throw new ArgumentException("Monkey batch must have at least 6 lines");
            var itemsLine = monkeyBatch.ElementAt(1);
            Items = new List<BigInteger>(itemsLine.Substring(itemsLine.IndexOf(':') + 1).Split(',').Select(BigInteger.Parse));
            WorryOperation = UpdateWorryOperation(monkeyBatch.ElementAt(2));
            Test = GetTest(monkeyBatch.Skip(3));
        }

        private static readonly Regex TestDivisibleRegex = new("Test: divisible by (?<val>\\d+)");
        private static readonly Regex TrueThrowRegex = new("If true: throw to monkey (?<val>\\d+)");
        private static readonly Regex FalseThrowRegex = new("If false: throw to monkey (?<val>\\d+)");

        private Action<BigInteger, List<Monkey>> GetTest(IEnumerable<string> monkeyTestLines)
        {
            var testLine = monkeyTestLines.First();
            var testMatch = TestDivisibleRegex.Match(testLine);
            var val = BigInteger.Parse(testMatch.Groups["val"].Value);
            var trueThrowIndex = int.Parse(TrueThrowRegex.Match(monkeyTestLines.ElementAt(1)).Groups["val"].Value);
            var falseThrowIndex = int.Parse(FalseThrowRegex.Match(monkeyTestLines.ElementAt(2)).Groups["val"].Value);
            return (worry, monks) =>
            {
                var remainder = BigInteger.Remainder(worry, val);
                if (remainder == BigInteger.Zero)
                    monks.ElementAt(trueThrowIndex).Items.Add(worry);
                else
                    monks.ElementAt(falseThrowIndex).Items.Add(worry);
            };
        }

        private static readonly Regex WorryOperationRegex = new("Operation: new = old (?<op>[*+]) (?<val>\\d+|old)");
        private Func<BigInteger, BigInteger> UpdateWorryOperation(string worryOperationLine)
        {
            var match = WorryOperationRegex.Match(worryOperationLine);
            if (match.Success)
            {
                var op = match.Groups["op"].Value;
                if (match.Groups["val"].Value == "old")
                    return op switch
                    {
                        "*" => x => x * x,
                        "+" => x => x + x,
                        _ => throw new ArgumentException("Unknown operation: " + op)
                    };
                else
                {
                    var val = BigInteger.Parse(match.Groups["val"].Value);
                    return op switch
                    {
                        "*" => x => x * val,
                        "+" => x => x + val,
                        _ => throw new ArgumentException("Unknown operation: " + op)
                    };
                }
            }
            throw new ArgumentException("Could not parse worry operation line: " + worryOperationLine);
        }
    }
}
