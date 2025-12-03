namespace AdventOfCode.Puzzles._2025.Day3;

public class ElevatorSolution2 : ElevatorBase
{
    protected override object DoSolve()
    {
        var banks = ReadInputFromFile();
        var bankSums = 0ul;
        foreach (var bank in banks)
        {
            if (!string.IsNullOrWhiteSpace(bank))
            {
                var highest = FindHighest(bank, 11);
                // Console.WriteLine($"highest: {highest}");
                if (string.IsNullOrWhiteSpace(highest))
                {
                    throw new InvalidOperationException("Could not find highest number");
                }
                bankSums += ulong.Parse(highest);
            }
        }
        return bankSums;
    }

    private string FindHighest(string bank, int remaining)
    {
        var highest = 0;
        var highestIndex = -1;
        for (int i = 0; i < bank.Length - remaining; i++)
        {
            var current = int.Parse(bank[i].ToString());
            if (current > highest)
            {
                highest = current;
                highestIndex = i;
            }
        }
        if (remaining == 0)
        {
            return highest.ToString();
        }
        return $"{highest}{FindHighest(bank.Substring(highestIndex + 1), remaining - 1)}";
    }
}
