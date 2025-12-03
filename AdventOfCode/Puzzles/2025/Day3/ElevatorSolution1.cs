namespace AdventOfCode.Puzzles._2025.Day3;

public class ElevatorSolution1 : ElevatorBase
{
    protected override object DoSolve()
    {
        var banks = ReadInputFromFile();
        var bankSums = 0;
        foreach (var bank in banks)
        {
            if (!string.IsNullOrWhiteSpace(bank))
            {
                var firstHighest = 0;
                var firstHighestIndex = -1;
                var secondHighest = 0;
                foreach (var (ch, index) in bank.Take(bank.Length - 1).Select((c, i) => (c.ToString(), i)))
                {
                    var value = int.Parse(ch);
                    if (value > firstHighest)
                    {
                        firstHighest = value;
                        firstHighestIndex = index;
                    }
                }
                for (var i = firstHighestIndex + 1; i < bank.Length; i++)
                {
                    var ch = bank[i].ToString();
                    var value = int.Parse(ch);
                    if (value > secondHighest)
                    {
                        secondHighest = value;
                    }
                }
                bankSums += int.Parse($"{firstHighest}{secondHighest}");
            }
        }
        return bankSums;
    }
}
